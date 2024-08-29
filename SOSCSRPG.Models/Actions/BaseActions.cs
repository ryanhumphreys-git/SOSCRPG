using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SOSCSRPG.Models;

namespace SOSCSRPG.Models.Actions
{
    public abstract class BaseActions
    {
        protected readonly GameItem _itemInUse;

        public event EventHandler<string> OnActionPerformed;

        protected BaseActions(GameItem itemInUse)
        {
            _itemInUse = itemInUse;
        }

        protected void ReportResult(string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }
    }
}
