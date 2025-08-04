# UK-DALE Energy Demo Construction Kit

Dieses Construction Kit modelliert die UK-DALE (UK Domestic Appliance-Level Electricity) Dataset für Energie-Demos und Prototyping.

## 🏗️ **Modell-Übersicht**

### **Kern-Entitäten**

#### **UkDaleHouse**
- Repräsentiert einen überwachten Haushalt
- Enthält demografische und technische Daten
- Verknüpft mit Geräten, Bewohnern und Tarifen

#### **UkDaleAppliance** 
- Einzelne Haushaltsgeräte mit Monitoring
- Kategorisiert nach Gerätetypen
- Verknüpft mit Messwerten und Anomalien

#### **UkDalePowerReading**
- Einzelne Strommessungen (6-Sekunden-Intervall)
- Unterstützt verschiedene Leistungsarten
- Qualitätsindikatoren für Datenvalidierung

#### **UkDaleEnergyTariff**
- Stromtarife mit verschiedenen Tarifmodellen
- Zeit-abhängige Preisstrukturen
- Unterstützung für Economy 7, Time-of-Use, etc.

#### **UkDaleOccupant**
- Bewohner-Demographics und Verhalten
- Energie-Bewusstsein und Anwesenheitsmuster
- Korrelation mit Verbrauchsmustern

#### **UkDaleEnergyAnomaly**
- Automatische Anomalie-Erkennung
- Verschiedene Anomalie-Typen und Schweregrade
- Nachverfolgung und Resolution

### **Erweiterte Features**

#### **UkDaleWeeklySummary** & **UkDaleDailySummary**
- Aggregierte Verbrauchsauswertungen
- Kostenberechnungen basierend auf Tarifen
- Peak/Off-Peak Analysen

## 📊 **Demo-Szenarien**

### **1. Household Energy Management**
```csharp
// Haushalt mit verschiedenen Geräten
var house = GenerateHouse(1);
var appliances = GenerateAppliances("house_1", 8);
var readings = GeneratePowerReadings("house_1_fridge", DateTime.Now, 24);
```

### **2. Tariff Comparison**
- Flat Rate vs. Time-of-Use Tarife
- Economy 7 Optimierung
- Dynamic Pricing Szenarien

### **3. Anomaly Detection**
- Ungewöhnliche Verbrauchsmuster
- Geräte-Ausfälle erkennen
- Energieverschwendung identifizieren

### **4. Behavioral Analysis**
- Korrelation Bewohnerverhalten ↔ Energieverbrauch
- Awareness-Level Impact
- Home-Office vs. Office-Worker Patterns

## 🚀 **Verwendung**

### **Demo-Daten generieren:**
```csharp
using Octo.Energy.Demo.DemoData;

// Haushalt erstellen
var house = UkDaleDemoDataGenerator.GenerateHouse(1);
await octomesh.create_entity("OctoEnergyDemo/UkDaleHouse", JsonSerializer.Serialize(house));

// Geräte hinzufügen
var appliances = UkDaleDemoDataGenerator.GenerateAppliances("house_1");
foreach(var appliance in appliances)
{
    await octomesh.create_entity("OctoEnergyDemo/UkDaleAppliance", JsonSerializer.Serialize(appliance));
}

// Stromverbrauch simulieren
var readings = UkDaleDemoDataGenerator.GeneratePowerReadings("house_1_fridge", DateTime.Now, 24);
foreach(var reading in readings)
{
    await octomesh.create_entity("OctoEnergyDemo/UkDalePowerReading", JsonSerializer.Serialize(reading));
}
```

### **Typische Queries:**
```csharp
// Alle Haushalte abfragen
var houses = await octomesh.query_entities("OctoEnergyDemo/UkDaleHouse");

// Geräte eines Haushalts
var appliances = await octomesh.navigate_associations(
    "OctoEnergyDemo/UkDaleHouse", 
    houseId, 
    "HouseAppliances", 
    "Inbound", 
    "OctoEnergyDemo/UkDaleAppliance"
);

// Aktuelle Anomalien
var anomalies = await octomesh.query_entities_simple(
    "OctoEnergyDemo/UkDaleEnergyAnomaly",
    simpleFilters: "{\"isResolved\": false}"
);
```

## 📈 **Realistische Daten-Charakteristiken**

### **Zeitabhängige Muster:**
- **Kettle**: Spitzen morgens (7-9h) und abends (17-19h)
- **TV**: Hauptsächlich abends (18-23h)  
- **Washing Machine**: Tagsüber an Werktagen
- **Fridge**: Kontinuierlich mit Schwankungen
- **Lights**: Dämmerung/Nacht

### **Geräte-Kategorien:**
1. **LightingAndOther** - Beleuchtung, Elektronik
2. **KitchenMajor** - Kühlschrank, Spülmaschine, Wasserkocher
3. **LaundryAndCleaning** - Waschmaschine, Trockner
4. **HeatingAndCooling** - Heizung, Klimaanlage
5. **Entertainment** - TV, Audio, Computer
6. **WaterHeating** - Warmwasserbereitung
7. **CookingAppliances** - Herd, Ofen

### **UK-Spezifische Features:**
- UK Postleitzahlen
- Economy 7 Tarife (7h Nachtstrom)
- Typische UK Haushaltsgeräte
- Pence/kWh Preisstruktur
- Property Types (Terraced, Semi-detached, etc.)

## 🎯 **Demo-Performance Tips**

- **Kleine Datasets**: 3-5 Haushalte, 1 Woche Daten (~10K Readings)
- **Mittlere Datasets**: 10-20 Haushalte, 1 Monat Daten (~500K Readings)  
- **Batch-Imports**: Nutze Bulk-Create für große Datenmengen
- **Indexing**: Timestamp-basierte Queries optimieren

## 📋 **Nächste Schritte**

1. **Bulk Data Import** - CSV-Import Tools
2. **Analytics Dashboard** - Web-Interface für Visualisierung
3. **ML Integration** - Predictive Models für Verbrauch
4. **Real-time Streaming** - Live-Daten Simulation
5. **Cost Optimization** - Tarif-Empfehlungen

---
*Basiert auf dem UK-DALE Dataset von Jack Kelly (Imperial College London)*
