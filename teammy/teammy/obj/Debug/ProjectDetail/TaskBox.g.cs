﻿#pragma checksum "..\..\..\ProjectDetail\TaskBox.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "0C107A6E45F9BD974ED7544D0258693AB9F6E010D6253C4E67D8907C24CB89D8"
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
using teammy.ProjectDetail;


namespace teammy.ProjectDetail {
    
    
    /// <summary>
    /// TaskBox
    /// </summary>
    public partial class TaskBox : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 119 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid taskGrid;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle rectProj;
        
        #line default
        #line hidden
        
        
        #line 130 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid threeDotsGrid;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnEditDelete;
        
        #line default
        #line hidden
        
        
        #line 139 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid statusGrid;
        
        #line default
        #line hidden
        
        
        #line 143 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnStatus;
        
        #line default
        #line hidden
        
        
        #line 146 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid priorityGrid;
        
        #line default
        #line hidden
        
        
        #line 147 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnPriority;
        
        #line default
        #line hidden
        
        
        #line 150 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox assigneeCombo;
        
        #line default
        #line hidden
        
        
        #line 165 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker taskDate;
        
        #line default
        #line hidden
        
        
        #line 166 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox taskNameTextBox;
        
        #line default
        #line hidden
        
        
        #line 168 "..\..\..\ProjectDetail\TaskBox.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel assigneeStackPanel;
        
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
            System.Uri resourceLocater = new System.Uri("/teammy;component/projectdetail/taskbox.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\ProjectDetail\TaskBox.xaml"
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
            
            #line 43 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.svItem_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 50 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.etItem_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 57 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.dlItem_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 68 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.PriorMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 73 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.PriorMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 78 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.PriorMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 87 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.StatusMenuItem_Click);
            
            #line default
            #line hidden
            
            #line 87 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.StatusMenuItem_MouseEnter);
            
            #line default
            #line hidden
            
            #line 87 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.StatusMenuItem_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 97 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.StatusMenuItem_Click);
            
            #line default
            #line hidden
            
            #line 97 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.StatusMenuItem_MouseEnter);
            
            #line default
            #line hidden
            
            #line 97 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.StatusMenuItem_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 107 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.StatusMenuItem_Click);
            
            #line default
            #line hidden
            
            #line 107 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).MouseEnter += new System.Windows.Input.MouseEventHandler(this.StatusMenuItem_MouseEnter);
            
            #line default
            #line hidden
            
            #line 107 "..\..\..\ProjectDetail\TaskBox.xaml"
            ((System.Windows.Controls.MenuItem)(target)).MouseLeave += new System.Windows.Input.MouseEventHandler(this.StatusMenuItem_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 10:
            this.taskGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 11:
            this.rectProj = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 12:
            this.threeDotsGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 13:
            this.btnEditDelete = ((System.Windows.Controls.Button)(target));
            
            #line 132 "..\..\..\ProjectDetail\TaskBox.xaml"
            this.btnEditDelete.Click += new System.Windows.RoutedEventHandler(this.btnEditDelete_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.statusGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 15:
            this.btnStatus = ((System.Windows.Controls.Button)(target));
            
            #line 143 "..\..\..\ProjectDetail\TaskBox.xaml"
            this.btnStatus.Click += new System.Windows.RoutedEventHandler(this.btnStatus_Click);
            
            #line default
            #line hidden
            return;
            case 16:
            this.priorityGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 17:
            this.btnPriority = ((System.Windows.Controls.Button)(target));
            
            #line 147 "..\..\..\ProjectDetail\TaskBox.xaml"
            this.btnPriority.Click += new System.Windows.RoutedEventHandler(this.btnPriority_Click);
            
            #line default
            #line hidden
            return;
            case 18:
            this.assigneeCombo = ((System.Windows.Controls.ComboBox)(target));
            
            #line 150 "..\..\..\ProjectDetail\TaskBox.xaml"
            this.assigneeCombo.DropDownClosed += new System.EventHandler(this.assigneeCombo_DropDownClosed);
            
            #line default
            #line hidden
            return;
            case 19:
            this.taskDate = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 20:
            this.taskNameTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 166 "..\..\..\ProjectDetail\TaskBox.xaml"
            this.taskNameTextBox.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.taskNameTextBox_TextChanged);
            
            #line default
            #line hidden
            return;
            case 21:
            this.assigneeStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

