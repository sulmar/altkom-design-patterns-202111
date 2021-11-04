using Stateless;
using System;

namespace StatePattern
{

    // Przykład zastosowania Maszyny Stanów Skończonych 

    // dotnet add package Stateless
    public class Lamp
    {
        public LampState State => machine.State;

        private StateMachine<LampState, LampTrigger> machine;

        // Wizualizacja - https://dreampuf.github.io/GraphvizOnline
        public string Graph => Stateless.Graph.UmlDotGraph.Format(machine.GetInfo());

        private float level = 0.2f;

        public Lamp()
        {
            machine = new StateMachine<LampState, LampTrigger>(LampState.Off);

            machine.Configure(LampState.Off)
                .Permit(LampTrigger.Push, LampState.On);

            machine.Configure(LampState.On)
                .Permit(LampTrigger.Push, LampState.Off)
                .Permit(LampTrigger.SemiPush, LampState.Power30)
                .OnEntry(() => Console.WriteLine("RTG"), "RTG")
                .OnExit(() => Console.WriteLine("bye"), "BYE");

            machine.Configure(LampState.Power30)
                .Permit(LampTrigger.Push, LampState.Off)
                .Permit(LampTrigger.SemiPush, LampState.Power60);

            machine.Configure(LampState.Power60)
                .Permit(LampTrigger.Push, LampState.Off)
                .PermitIf(LampTrigger.SemiPush, LampState.Power90, ()=> level > 0.5f);

            machine.Configure(LampState.Power90)
                .Permit(LampTrigger.Push, LampState.Off);

            // Śledzenie przejść
            machine
                .OnTransitioned(t => Console.WriteLine($"{t.Source} -> {t.Destination}"));

            //    .Permit(LampTrigger.SemiPush, LampState.On);

        }


        public void PowerOn()
        {
            machine.Fire(LampTrigger.Push);
        }

        public void PowerOff()
        {
            machine.Fire(LampTrigger.Push);
        }

        public void SemiPush() => machine.Fire(LampTrigger.SemiPush);

        public bool CanSemiPush => machine.CanFire(LampTrigger.SemiPush);

    }


}
