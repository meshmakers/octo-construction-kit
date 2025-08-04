using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Octo.Energy.Demo.DemoData;

namespace Octo.Energy.Demo;

/// <summary>
/// Demo script for creating UK-DALE energy consumption scenarios
/// </summary>
public class UkDaleDemoScript
{
    // Placeholder for OctoMesh client - replace with actual implementation
    private readonly dynamic _octomesh;
    
    public UkDaleDemoScript(dynamic octomesh)
    {
        _octomesh = octomesh;
    }

    /// <summary>
    /// Create a complete demo scenario with multiple households
    /// </summary>
    public async Task CreateEnergyCommunityDemo(int householdCount = 5)
    {
        Console.WriteLine($"🏗️ Creating UK-DALE Energy Community Demo with {householdCount} households...");
        
        // 1. Create Energy Tariffs
        await CreateEnergyTariffs();
        
        // 2. Create Households with different profiles
        var houseIds = new List<string>();
        for (int i = 1; i <= householdCount; i++)
        {
            var houseId = await CreateHouseholdWithProfile(i);
            houseIds.Add(houseId);
        }
        
        // 3. Generate realistic power consumption data
        await GeneratePowerConsumption(houseIds);
        
        // 4. Create some interesting anomalies for demo
        await CreateDemoAnomalies(houseIds);
        
        Console.WriteLine("✅ Demo data creation completed!");
        await PrintDemoStats();
    }

    private async Task CreateEnergyTariffs()
    {
        Console.WriteLine("📊 Creating energy tariffs...");
        
        var tariffs = new[]
        {
            UkDaleDemoDataGenerator.GenerateEnergyTariff("FLAT_STANDARD"),
            UkDaleDemoDataGenerator.GenerateEnergyTariff("ECONOMY_7"),
            UkDaleDemoDataGenerator.GenerateEnergyTariff("TIME_OF_USE")
        };

        foreach (var tariff in tariffs)
        {
            await _octomesh.create_entity("OctoEnergyDemo/UkDaleEnergyTariff", JsonSerializer.Serialize(tariff));
        }
    }

    private async Task<string> CreateHouseholdWithProfile(int householdNumber)
    {
        Console.WriteLine($"🏠 Creating household {householdNumber}...");
        
        // Create house
        var house = UkDaleDemoDataGenerator.GenerateHouse(householdNumber);
        var houseId = house.GetType().GetProperty("houseId")?.GetValue(house)?.ToString();
        
        await _octomesh.create_entity("OctoEnergyDemo/UkDaleHouse", JsonSerializer.Serialize(house));
        
        // Create appliances with different profiles based on house number
        var applianceCount = householdNumber switch
        {
            1 => 6,  // Small household
            2 => 8,  // Average household  
            3 => 10, // Large household
            4 => 7,  // Tech-heavy household
            _ => 8   // Default
        };
        
        var appliances = UkDaleDemoDataGenerator.GenerateAppliances(houseId, applianceCount);
        foreach (var appliance in appliances)
        {
            await _octomesh.create_entity("OctoEnergyDemo/UkDaleAppliance", JsonSerializer.Serialize(appliance));
        }
        
        // Create occupants
        var occupantNames = GetOccupantNames(householdNumber);
        foreach (var name in occupantNames)
        {
            var occupant = UkDaleDemoDataGenerator.GenerateOccupant(name);
            await _octomesh.create_entity("OctoEnergyDemo/UkDaleOccupant", JsonSerializer.Serialize(occupant));
        }
        
        return houseId;
    }

    private async Task GeneratePowerConsumption(List<string> houseIds)
    {
        Console.WriteLine("⚡ Generating power consumption data...");
        
        var startTime = DateTime.Now.AddDays(-7); // Last week
        
        foreach (var houseId in houseIds)
        {
            // Generate data for main appliances
            var applianceTypes = new[] { "fridge", "washing_machine", "tv", "kettle", "lights" };
            
            foreach (var appType in applianceTypes)
            {
                var applianceId = $"{houseId}_{appType}";
                
                // Generate power readings for the last week
                for (int day = 0; day < 7; day++)
                {
                    var dayStart = startTime.AddDays(day);
                    var readings = UkDaleDemoDataGenerator.GeneratePowerReadings(
                        applianceId, dayStart, 24);
                    
                    // Sample only every 10th reading to keep demo data manageable
                    for (int i = 0; i < readings.Count; i += 10)
                    {
                        await _octomesh.create_entity("OctoEnergyDemo/UkDalePowerReading", 
                            JsonSerializer.Serialize(readings[i]));
                    }
                }
            }
        }
    }

    private async Task CreateDemoAnomalies(List<string> houseIds)
    {
        Console.WriteLine("🚨 Creating demo anomalies...");
        
        var anomalies = new[]
        {
            new
            {
                detectionTime = DateTime.Now.AddHours(-2).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                anomalyType = 1, // UnexpectedHigh
                severityLevel = 3, // High
                expectedValue = 150.0,
                actualValue = 2500.0,
                deviationPercent = 1566.7,
                isResolved = false,
                description = "Fridge consuming unusually high power - possible defrost cycle malfunction"
            },
            new
            {
                detectionTime = DateTime.Now.AddHours(-5).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                anomalyType = 3, // ApplianceAlwaysOn
                severityLevel = 2, // Medium
                expectedValue = 0.0,
                actualValue = 85.0,
                deviationPercent = double.PositiveInfinity,
                isResolved = true,
                resolvedTime = DateTime.Now.AddHours(-1).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                description = "TV appears to be always on - standby mode detected"
            }
        };

        foreach (var anomaly in anomalies)
        {
            await _octomesh.create_entity("OctoEnergyDemo/UkDaleEnergyAnomaly", JsonSerializer.Serialize(anomaly));
        }
    }

    private async Task PrintDemoStats()
    {
        Console.WriteLine("\n📈 Demo Statistics:");
        
        // This would need actual OctoMesh query implementation
        Console.WriteLine("   • Houses: 5");
        Console.WriteLine("   • Appliances: ~40");
        Console.WriteLine("   • Power Readings: ~14,000");
        Console.WriteLine("   • Energy Tariffs: 3");
        Console.WriteLine("   • Occupants: ~12");
        Console.WriteLine("   • Anomalies: 2");
        Console.WriteLine("\n🎯 Ready for energy analytics demos!");
    }

    private static List<string> GetOccupantNames(int householdNumber) => householdNumber switch
    {
        1 => new List<string> { "Alice Johnson" },
        2 => new List<string> { "Bob Smith", "Carol Smith" },
        3 => new List<string> { "David Brown", "Emma Brown", "Frank Brown", "Grace Brown" },
        4 => new List<string> { "Henry Wilson", "Isabel Wilson" },
        _ => new List<string> { "John Doe", "Jane Doe" }
    };
}

/// <summary>
/// Entry point for demo execution
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("🌟 UK-DALE Energy Demo Data Generator");
        Console.WriteLine("=====================================\n");
        
        // TODO: Initialize actual OctoMesh client
        // var octomesh = new OctomeshClient(connectionString);
        dynamic octomesh = null; // Placeholder
        
        var demo = new UkDaleDemoScript(octomesh);
        
        try
        {
            await demo.CreateEnergyCommunityDemo(5);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Demo creation failed: {ex.Message}");
        }
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
