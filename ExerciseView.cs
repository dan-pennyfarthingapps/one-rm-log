using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Dialog;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

using PennyFarElements;

namespace onermlog
{
	public class ExerciseView : CustomDialogViewController
	{
		public ExerciseView () : base(new RootElement("root"))
		{
		}
	}
}

