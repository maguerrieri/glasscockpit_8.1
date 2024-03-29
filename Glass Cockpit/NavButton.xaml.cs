﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Glass_Cockpit {
	public sealed partial class NavButton : Button {
		public NavButton() {
			this.InitializeComponent();
		}

		public string Text {
			get {
				return this.label.Text;
			}
			set {
				this.label.Text = value;
			}
		}
		public Symbol Symbol {
			get {
				return this.icon.Symbol;
			}
			set {
				this.icon.Symbol = value;
			}
		}
	}
}
