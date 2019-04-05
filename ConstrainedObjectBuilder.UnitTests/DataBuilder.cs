using Decider.Csp.BaseTypes;
using Decider.Csp.Integer;
using System;

namespace ConstrainedObjectBuilder.UnitTests
{
    class DataBuilder : ObjectBuilder<Data>
    {
        public override Data Build()
        {
            IState<int> state = new StateInteger(Variables.Values, Constraints);
            state.StartSearch(out _);

            var startDate = DateTime.Now;
            var data = new Data();

            foreach (var variable in Variables)
            {
                variable.Key.SetValue(data, startDate.AddDays(variable.Value.Value));
            }

            return data;
        }
    }
}
