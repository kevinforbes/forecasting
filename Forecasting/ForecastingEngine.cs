using System;
using System.Collections.Generic;
using System.Linq;

namespace Forecasting
{
    public class ForecastingEngine
    {

        public DeviceForecast CalculateDeviceConsumableForecast(List<MonitoringMessage> messages, Guid deviceId)
        {
            DeviceForecast forecast = new DeviceForecast();

            foreach (MonitoringMessage message
                in messages.OrderByDescending(x => x.LastReportedAt))
            {

                //if level is greater than last reported, they probably replaced the cartridge

                //calculate average daily use, keep in mind it might not have been reported every day (some days may have been skipped)

                //calculate Days remaining until ten percent left

                //if level is 0, stop counting usage towards average
            }

            return forecast;
        }
    }

    public class DeviceForecast
    {
        public string DeviceId { get; set; }
        public DateTime TimeStamp { get; set; }
        public List<ConsumableForecast> ConsumableForecasts { get; set; }
    }

    public class ConsumableForecast
    {
        public string ConsumableId { get; set; }
        public int AverageDailyUse { get; set; }
        public int DaysRemainingToTenPercent { get; set; }
        public int CurrentLevel { get; set; }
    }

    public class MonitoringMessage
    {
        public Guid DeviceId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ManufacturerId { get; set; }
        public Guid DealerId { get; set; }
        public string Name { get; set; }
        public DateTime LastReportedAt { get; set; }
        public DateTime FirstReportedAt { get; set; }
        public virtual ICollection<Consumable> Consumables { get; set; }


        public string ProviderDeviceId { get; set; }
        public string ProviderCustomerId { get; set; }
        public string DeviceModelNumber { get; set; }
        public string Manufacturer { get; set; }
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
        public string CustomerName { get; set; }
        public Guid GroupId { get; set; }
        public string Status { get; set; }
        public string Type { get; set; } //ex:network
        public string ManagementStatus { get; set; } //ex:Managed
        public string LicenseStatus { get; set; } //ex:Full
        public string AssetNumber { get; set; }
        public string SerialNumber { get; set; }
        public string Location { get; set; }
        public string DeviceDescription { get; set; }
        public DateTime TimeStamp { get; set; }
    }


    public class Consumable //ex:PrintFleet calls this ColorSupplies
    {
        public string Id { get; set; }
        public string Name { get; set; } //ex:Cyan, Magenta, Black
        public string Label { get; set; } //ex:TonerLevel_Black, TonerLevel_Cyan
        public string Color { get; set; }
        public string OEMPartNumber { get; set; }
        public string Type { get; set; } //ex:Toner, Drum
        public int Level { get; set; }
    }

}
