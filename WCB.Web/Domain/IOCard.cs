using System;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using FluentAssertions;
using Microsoft.Framework.ConfigurationModel;

namespace WCB.Web.Domain
{
    class IOCard : IScrewAndAirIO, ISensor
    {
        private readonly ILog _logger;
        public IOCard(ILog logger, Configuration serialPortConfig)
        {
            _logger = logger;
            _serialPort = new SerialPort
            {
                BaudRate = serialPortConfig.Get<int>("SerialPort:Baud"),
                PortName = serialPortConfig.Get<string>("SerialPort:Port"),
                WriteTimeout = 1000,
                ReadTimeout = 1000
            };
        }

        private State _screwState = State.Disabled;
        private State _airState = State.Disabled;
        private readonly SerialPort _serialPort;

        public State GetScrew() => _screwState;

        public void SetScrew(State state)
        {
            if(_screwState != state)
                _logger.Write($"Updated screw state '{state}'.");

            _screwState = state;
            Retry(() => ExecuteCommand($"SET 2.T2 {ConvertState(state)}"));
        }

        public State GetAir() => _airState;

        public void SetAir(State state)
        {
            if(_airState != state)
                _logger.Write($"Updated air state '{state}'.");

            Retry(() => ExecuteCommand($"SET 2.T1 {ConvertState(state)}"));

            _airState = state;
        }

        private string ConvertState(State state)
        {
            return state == State.Enabled ? "HIGH" : "LOW";
        }

        private T Retry<T>(Func<T> call)
        {
            try
            {
                return call.Invoke();
            }
            catch (Exception ex)
            {
                Thread.Sleep(10);
                _logger.Write($"Got exception from io card '{ex.Message}', trying retry.");
                return call.Invoke();
            }
        }

        private string ExecuteCommand(string command)
        {
            lock (_serialPort)
            {
                try
                {
                    _serialPort.Open();
                    _serialPort.ReadExisting();
                    _serialPort.WriteLine(command);

                    Thread.Sleep(75);

                    var response = _serialPort.ReadExisting();

                    if (response.Contains("ERROR"))
                        throw new IOCardException($"IOCard responsed with error, messages: '{response}'");

                    return response;
                }
                finally
                {
                    _serialPort.Close();
                }
            }
        }

        public SensorValue GetSensor()
        {
            var response = Retry(() => ExecuteCommand("ADC 7.T0.ADC0"));
            return GetSensorValueWithRetry(response);
        }

        private static SensorValue GetSensorValueWithRetry(string response)
        {
            var regex = new Regex(@".*?\r\n(\d+).*");

            if (regex.Match(response) != Match.Empty)
                return new SensorValue(int.Parse(regex.Split(response)[1]));

            throw new IOCardException($"Cannot parse response '{response}' as sensor value.");
        }
    }
}