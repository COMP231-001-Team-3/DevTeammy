﻿#pragma checksum "..\..\..\ProjectDetail\NewCategory.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CBEDA3DAD51AD076F5C3A3CEE1D8F5591CFCFFC67420813BADE0F13BE1539B92"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using teammy;


namespace teammy {
    
    
    /// <summary>
    /// NewCategory
    /// </summary>
    public partial class NewCategory : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 65 "..\..\..\ProjectDetail\NewCategory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid containerGrid;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\ProjectDetail\NewCategory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle categoryRect;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\ProjectDetail\NewCategory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtCategroyName;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\ProjectDetail\NewCategory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button addTaskButton;
        
        #line default
        #line hidden
        
        
        #line 84 "..\..\..\ProjectDetail\NewCategory.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnCloseC;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/teammy;component/projectdetail/newcategory.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ProjectDetail\NewCategory.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.containerGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.categoryRect = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 3:
            this.txtCategroyName = ((System.Windows.Controls.TextBox)(target));
            
            #line 68 "..\..\..\ProjectDetail\NewCategory.xaml"
            this.txtCategroyName.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.txtCategroyName_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.addTaskButton = ((System.Windows.Controls.Button)(target));
            
            #line 74 "..\..\..\ProjectDetail\NewCategory.xaml"
            this.addTaskButton.Click += new System.Windows.RoutedEventHandler(this.addTask);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnCloseC = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

