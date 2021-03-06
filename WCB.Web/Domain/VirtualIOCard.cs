﻿namespace WCB.Web.Domain
{
    class VirtualIOCard : IScrewAndAirIO
    {
        private readonly ILog _logger;
        public VirtualIOCard(ILog logger)
        {
            _logger = logger;
        }

        private State _screwState = State.Disabled;
        private State _airState = State.Disabled;

        public State GetScrew() => _screwState;

        public void SetScrew(State state)
        {
            if (_screwState != state)
                _logger.Write($"Updated screw state '{state}'.");

            _screwState = state;
        }

        public State GetAir() => _airState;

        public void SetAir(State state)
        {
            if (_airState != state)
                _logger.Write($"Updated air state '{state}'.");

            _airState = state;
        }
    }
}