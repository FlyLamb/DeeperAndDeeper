using UnityEngine;
using Unity.VisualScripting;

namespace BoltStuff
{

    [UnitCategory("Events/TNRK")]
    [RenamedFrom("Button")]
    [UnitTitle("Red Button Event")]
    public sealed class Bolt_RedButton : MachineEventUnit<EmptyEventArgs>
    {

        public new class Data : EventUnit<EmptyEventArgs>.Data
        {
            public RedButton executor;
        }

        public override IGraphElementData CreateData()
        {
            return new Data();
        }

        protected override string hookName => EventHooks.Update;

        [DoNotSerialize]
        [PortLabel("Button")]
        public ValueInput executor { get; private set; }


        protected override void Definition()
        {
            base.Definition();

            executor = ValueInput<RedButton>(nameof(executor), null);
        }

        public override void StartListening(GraphStack stack)
        {
            base.StartListening(stack);

            var data = stack.GetElementData<Data>(this);
        }

        protected override bool ShouldTrigger(Flow flow, EmptyEventArgs args)
        {
            var data = flow.stack.GetElementData<Data>(this);
            var _executor = flow.GetValue<RedButton>(executor);
            data.executor = _executor;
            return _executor.__SHOULDTRIGGER;
        }
    }
}
