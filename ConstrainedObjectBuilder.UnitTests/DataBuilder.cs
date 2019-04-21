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
      state.StartSearch(out StateOperationResult result);

      if (result == StateOperationResult.Unsatisfiable)
      {
        throw new InvalidOperationException();
      }

      var data = new Data();

      foreach (var variable in Variables)
      {
        if (variable.Key.PropertyType == typeof(DateTime))
        {
          variable.Key.SetValue(data, DateHelper.ToDateTime(variable.Value.Value));
        }
        else
        {
          variable.Key.SetValue(data, variable.Value.Value);
        }
      }

      return data;
    }
  }
}
