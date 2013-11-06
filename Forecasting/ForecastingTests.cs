using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Forecasting
{
    [TestClass]
    public class ForecastingTests
    {

        [TestMethod]
        public void Given30Messages1DayApart_StartingAt100AndDecreasingBy1EachDayFor29Days_AverageDailyUseEquals1()
        {
            Guid customerId = Guid.NewGuid();
            Guid deviceId = Guid.NewGuid();

            List<MonitoringMessage> messages =
                new MonitoringMessageListBuilder(customerId, deviceId).AListOfMessages(29, 1, 100, 1).Build();

            ForecastingEngine engine = new ForecastingEngine();
            DeviceForecast forecast = engine.CalculateDeviceConsumableForecast(messages, deviceId);

            // 4 consumables for this device, all are decreasing at the same rate of 1 percent per day 
            // which means they have 50 days left until hitting 10 percent
            Assert.AreEqual(4, forecast.ConsumableForecasts.Count);
            Assert.IsFalse(forecast.ConsumableForecasts.Any(x => x.CurrentLevel != 61));
            Assert.IsFalse(forecast.ConsumableForecasts.Any(x => x.AverageDailyUse != 1));
            Assert.IsFalse(forecast.ConsumableForecasts.Any(x => x.DaysRemainingToTenPercent != 50));
        }

        [TestMethod]
        public void Given30Messages2DaysApart_StartingAt100AndDecreasingBy2BetweenEachMessage_DaysLeftEquals59()
        {
            Guid customerId = Guid.NewGuid();
            Guid deviceId = Guid.NewGuid();

            List<MonitoringMessage> messages =
                new MonitoringMessageListBuilder(customerId, deviceId).AListOfMessages(29, 1, 100, 1).Build();

            ForecastingEngine engine = new ForecastingEngine();
            DeviceForecast forecast = engine.CalculateDeviceConsumableForecast(messages, deviceId);

            // 4 consumables for this device, all are decreasing at the same rate of 1 percent per day 
            // which means they have 50 days left until hitting 10 percent
            Assert.AreEqual(4, forecast.ConsumableForecasts.Count);
            Assert.IsFalse(forecast.ConsumableForecasts.Any(x => x.CurrentLevel != 61));
            Assert.IsFalse(forecast.ConsumableForecasts.Any(x => x.AverageDailyUse != 1));
            Assert.IsFalse(forecast.ConsumableForecasts.Any(x => x.DaysRemainingToTenPercent != 50));
        }

        public void Given2Messages30DaysApart_StartingAt100AndDecreasingBy30BetweenMessages_AverageDailyUseEquals1()
        {
            Guid customerId = Guid.NewGuid();
            Guid deviceId = Guid.NewGuid();

            List<MonitoringMessage> messages =
                new MonitoringMessageListBuilder(customerId, deviceId).AListOfMessages(2, 30, 100, 29).Build();


            ForecastingEngine engine = new ForecastingEngine();
            DeviceForecast forecast = engine.CalculateDeviceConsumableForecast(messages, deviceId);


            // 4 consumables for this device, all are decreasing at the same rate of 1 percent per day 
            // which means they have 50 days left until hitting 10 percent
            Assert.AreEqual(4, forecast.ConsumableForecasts.Count);
            Assert.IsFalse(forecast.ConsumableForecasts.Any(x => x.CurrentLevel != 61));
            Assert.IsFalse(forecast.ConsumableForecasts.Any(x => x.AverageDailyUse != 1));
            Assert.IsFalse(forecast.ConsumableForecasts.Any(x => x.DaysRemainingToTenPercent != 50));
        }

    }

    public class MonitoringMessageListBuilder
    {
        private readonly Guid _customerId;
        private readonly Guid _deviceId;
        private List<MonitoringMessage> _messages;

        public MonitoringMessageListBuilder(Guid customerId, Guid deviceId)
        {
            _customerId = customerId;
            _deviceId = deviceId;
            _messages = new List<MonitoringMessage>();
        }

        public MonitoringMessageListBuilder AListOfMessages(int numberOfMessages, int daysApart, int startingPercentage,
            int decreaseByPerMessage)
        {
            DateTime lastDateUsed = DateTime.Today;
            int lastPercentageUsed = startingPercentage;

            for (int i = 0; i < numberOfMessages; i++)
            {
                DateTime dateToUse = lastDateUsed.AddDays(daysApart);
                int percentage = lastPercentageUsed - decreaseByPerMessage;

                _messages.Add(
                    new MonitoringMessageBuilder(_customerId, dateToUse)
                        .WithCyanMagentaYellowAndBlackConsumables(percentage, percentage, percentage, percentage)
                        .Build());

                lastDateUsed = dateToUse;
                lastPercentageUsed = percentage;
            }

            return this;
        }

        public List<MonitoringMessage> Build()
        {
            return _messages;
        }
    }

    public class MonitoringMessageBuilder
    {
        private readonly MonitoringMessage _message;

        public MonitoringMessageBuilder(Guid customerId, DateTime reportedDate)
        {
            _message = new MonitoringMessage
            {
                CustomerId = customerId,
                ProviderDeviceId = Guid.NewGuid().ToString(),
                DealerId = Guid.NewGuid(),
                DeviceDescription = "Brother MFC 8two9",
                DeviceModelNumber = "MFC829",
                LastReportedAt = reportedDate,
                TimeStamp = DateTime.UtcNow
            };
        }

        public MonitoringMessageBuilder WithConsumable(string color, int level)
        {
            _message.Consumables.Add(
                new Consumable
                {
                    Color = color,
                    Level = level,
                    Id = Guid.NewGuid().ToString(),
                    Type = "Toner"
                });

            return this;
        }

        public MonitoringMessageBuilder WithCyanMagentaYellowAndBlackConsumables(int cyanLevel, int magentaLevel,
            int yellowLevel, int blackLevel)
        {
            _message.Consumables.Add(
                new Consumable
                {
                    Color = System.Drawing.Color.Cyan.ToString(),
                    Level = cyanLevel,
                    Id = Guid.NewGuid().ToString(),
                    Type = "Toner"
                });

            _message.Consumables.Add(
                new Consumable
                {
                    Color = System.Drawing.Color.Magenta.ToString(),
                    Level = magentaLevel,
                    Id = Guid.NewGuid().ToString(),
                    Type = "Toner"
                });

            _message.Consumables.Add(
                new Consumable
                {
                    Color = System.Drawing.Color.Yellow.ToString(),
                    Level = yellowLevel,
                    Id = Guid.NewGuid().ToString(),
                    Type = "Toner"
                });

            _message.Consumables.Add(
                new Consumable
                {
                    Color = System.Drawing.Color.Black.ToString(),
                    Level = blackLevel,
                    Id = Guid.NewGuid().ToString(),
                    Type = "Toner"
                });

            return this;
        }

        public MonitoringMessage Build()
        {
            return _message;
        }
    }
}