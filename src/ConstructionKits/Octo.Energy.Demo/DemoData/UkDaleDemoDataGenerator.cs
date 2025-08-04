using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Octo.Energy.Demo.DemoData;

/// <summary>
/// Generates realistic UK-DALE demo data for energy consumption scenarios
/// </summary>
public static class UkDaleDemoDataGenerator
{
    private static readonly Random Random = new();
    
    private static readonly List<string> UkPostalCodes = new()
    {
        "SW1A 1AA", "M1 1AA", "B1 1AA", "LS1 1AA", "EH1 1AA",
        "CF1 1AA", "BT1 1AA", "L1 1AA", "S1 1AA", "NE1 1AA"
    };
    
    private static readonly Dictionary<string, (double min, double max)> AppliancePowerRanges = new()
    {
        { "fridge", (100, 200) },
        { "washing_machine", (400, 2500) },
        { "dishwasher", (1800, 2500) },
        { "kettle", (2000, 3000) },
        { "microwave", (800, 1500) },
        { "tv", (100, 300) },
        { "laptop", (50, 100) },
        { "lights", (10, 100) },
        { "boiler", (3000, 15000) },
        { "oven", (2000, 5000) }
    };

    /// <summary>
    /// Generate a UK-DALE house with realistic parameters
    /// </summary>
    public static object GenerateHouse(int houseId)
    {
        var startDate = DateTime.Now.AddYears(-2).AddDays(Random.Next(-365, 0));
        var endDate = startDate.AddDays(Random.Next(30, 1095)); // 1 month to 3 years
        
        return new
        {
            houseId = $"house_{houseId}",
            name = $"UK-DALE House {houseId}",
            description = $"Energy monitored household {houseId} from UK-DALE dataset",
            occupantCount = Random.Next(1, 5),
            floorArea = Random.Next(50, 300),
            recordingStartDate = startDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            recordingEndDate = endDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            postalCode = UkPostalCodes[Random.Next(UkPostalCodes.Count)],
            propertyType = Random.Next(1, 7), // PropertyType enum values
            yearBuilt = Random.Next(1950, 2020)
        };
    }

    /// <summary>
    /// Generate appliances for a house
    /// </summary>
    public static List<object> GenerateAppliances(string houseId, int count = 8)
    {
        var appliances = new List<object>();
        var applianceNames = AppliancePowerRanges.Keys.Take(count).ToList();
        
        for (int i = 0; i < applianceNames.Count; i++)
        {
            var applianceName = applianceNames[i];
            var powerRange = AppliancePowerRanges[applianceName];
            
            appliances.Add(new
            {
                applianceId = $"{houseId}_{applianceName}",
                name = FormatApplianceName(applianceName),
                description = $"{FormatApplianceName(applianceName)} in {houseId}",
                channelNumber = i + 1,
                category = GetApplianceCategory(applianceName),
                roomLocation = GetRoomLocation(applianceName),
                manufacturer = GetRandomManufacturer(applianceName),
                modelName = $"Model-{Random.Next(100, 999)}",
                ratedPower = Random.Next((int)powerRange.min, (int)powerRange.max),
                installationDate = DateTime.Now.AddYears(-Random.Next(1, 10)).ToString("yyyy-MM-ddTHH:mm:ssZ"),
                samplingFrequency = 6
            });
        }
        
        return appliances;
    }

    /// <summary>
    /// Generate power readings for an appliance over a time period
    /// </summary>
    public static List<object> GeneratePowerReadings(string applianceId, DateTime startTime, int durationHours = 24)
    {
        var readings = new List<object>();
        var current = startTime;
        var endTime = startTime.AddHours(durationHours);
        
        // Get base power for this appliance type
        var applianceType = applianceId.Split('_').Last();
        var basePower = AppliancePowerRanges.ContainsKey(applianceType) 
            ? (AppliancePowerRanges[applianceType].min + AppliancePowerRanges[applianceType].max) / 2
            : 100;

        while (current < endTime)
        {
            var power = GenerateRealisticPower(applianceType, current, basePower);
            
            readings.Add(new
            {
                timestamp = current.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                powerWatts = Math.Round(power, 2),
                powerType = 1, // Active power
                dataQuality = Random.Next(1, 5) == 1 ? 2 : 1, // Occasionally poor quality
                energyKwh = Math.Round(power * (6.0 / 3600), 4) // 6-second intervals
            });
            
            current = current.AddSeconds(6); // UK-DALE sampling frequency
        }
        
        return readings;
    }

    /// <summary>
    /// Generate energy tariff data
    /// </summary>
    public static object GenerateEnergyTariff(string tariffCode)
    {
        var tariffTypes = new[] { 1, 2, 3 }; // FlatRate, Economy7, TimeOfUse
        var tariffType = tariffTypes[Random.Next(tariffTypes.Length)];
        
        var baseRate = Random.Next(12, 25); // UK pence per kWh
        
        return new
        {
            tariffCode = tariffCode,
            name = $"Tariff {tariffCode}",
            description = $"Energy tariff {tariffCode} for UK households",
            rateType = tariffType,
            baseRate = baseRate,
            highRate = tariffType > 1 ? baseRate + Random.Next(5, 15) : (double?)null,
            lowRate = tariffType > 1 ? baseRate - Random.Next(3, 8) : (double?)null,
            standingCharge = Random.Next(20, 50), // Daily standing charge in pence
            validFrom = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-ddTHH:mm:ssZ"),
            validTo = DateTime.Now.AddMonths(12).ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
    }

    /// <summary>
    /// Generate occupant data
    /// </summary>
    public static object GenerateOccupant(string name)
    {
        return new
        {
            name = name,
            description = $"Household occupant: {name}",
            ageGroup = Random.Next(1, 7), // AgeGroup enum
            occupationType = Random.Next(1, 7), // OccupationType enum
            energyAwarenessLevel = Random.Next(1, 5), // AwarenessLevel enum
            homePresenceHours = Random.Next(8, 20),
            smartThermostat = Random.Next(0, 2) == 1,
            smartMeterAccess = Random.Next(0, 2) == 1
        };
    }

    private static double GenerateRealisticPower(string applianceType, DateTime time, double basePower)
    {
        var hour = time.Hour;
        var dayOfWeek = time.DayOfWeek;
        
        // Apply time-of-day patterns
        var timeMultiplier = applianceType switch
        {
            "kettle" => (hour >= 7 && hour <= 9) || (hour >= 17 && hour <= 19) ? 
                Random.NextDouble() * 2 : Random.NextDouble() * 0.1,
            "washing_machine" => (hour >= 9 && hour <= 17) && dayOfWeek != DayOfWeek.Sunday ? 
                Random.NextDouble() : Random.NextDouble() * 0.1,
            "tv" => (hour >= 18 && hour <= 23) ? 
                0.8 + Random.NextDouble() * 0.4 : Random.NextDouble() * 0.3,
            "fridge" => 0.7 + Random.NextDouble() * 0.6, // Always on with variation
            "lights" => (hour <= 7 || hour >= 18) ? 
                0.5 + Random.NextDouble() * 0.8 : Random.NextDouble() * 0.2,
            _ => Random.NextDouble()
        };
        
        return Math.Max(0, basePower * timeMultiplier);
    }

    private static string FormatApplianceName(string name)
    {
        return string.Join(" ", name.Split('_').Select(word => 
            char.ToUpper(word[0]) + word.Substring(1).ToLower()));
    }

    private static int GetApplianceCategory(string applianceName)
    {
        return applianceName switch
        {
            "lights" => 1, // LightingAndOther
            "fridge" or "dishwasher" or "kettle" or "microwave" => 2, // KitchenMajor
            "washing_machine" => 3, // LaundryAndCleaning
            "boiler" => 4, // HeatingAndCooling
            "tv" or "laptop" => 5, // Entertainment  
            "oven" => 7, // CookingAppliances
            _ => 1
        };
    }

    private static string GetRoomLocation(string applianceName)
    {
        return applianceName switch
        {
            "fridge" or "dishwasher" or "kettle" or "microwave" or "oven" => "Kitchen",
            "washing_machine" => "Utility Room",
            "tv" => "Living Room",
            "laptop" => "Study",
            "boiler" => "Utility Room",
            "lights" => "Various",
            _ => "Unknown"
        };
    }

    private static string GetRandomManufacturer(string applianceName)
    {
        var manufacturers = applianceName switch
        {
            "fridge" => new[] { "Bosch", "Samsung", "LG", "Hotpoint" },
            "washing_machine" => new[] { "Bosch", "Hotpoint", "Indesit", "Samsung" },
            "tv" => new[] { "Samsung", "LG", "Sony", "Panasonic" },
            "laptop" => new[] { "Dell", "HP", "Lenovo", "Apple" },
            _ => new[] { "Generic", "Unknown", "Various" }
        };
        
        return manufacturers[Random.Next(manufacturers.Length)];
    }
}
