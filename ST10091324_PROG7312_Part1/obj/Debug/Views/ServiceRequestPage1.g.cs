﻿#pragma checksum "..\..\..\Views\ServiceRequestPage1.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "4FB7467C7EE2A5584AC515756006BD6D5E1D36162208E7677DE868AB893AB1E8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using ST10091324_PROG7312_Part1.Converter;
using ST10091324_PROG7312_Part1.Views;
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


namespace ST10091324_PROG7312_Part1.Views {
    
    
    /// <summary>
    /// ServiceRequestPage1
    /// </summary>
    public partial class ServiceRequestPage1 : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\Views\ServiceRequestPage1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel MessageStackPanelServiceRequest;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Views\ServiceRequestPage1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.PackIcon MessagetHeaderIconServiceRequest;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Views\ServiceRequestPage1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MessageHeaderServiceRequest;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\..\Views\ServiceRequestPage1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid bottomHeaderGrid;
        
        #line default
        #line hidden
        
        
        #line 60 "..\..\..\Views\ServiceRequestPage1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid searchGrid;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\Views\ServiceRequestPage1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox requestSearch;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\Views\ServiceRequestPage1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateServiceRequestBtn;
        
        #line default
        #line hidden
        
        
        #line 77 "..\..\..\Views\ServiceRequestPage1.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox ServiceRequestsList;
        
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
            System.Uri resourceLocater = new System.Uri("/ST10091324_PROG7312_Part1;component/views/servicerequestpage1.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ServiceRequestPage1.xaml"
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
            this.MessageStackPanelServiceRequest = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.MessagetHeaderIconServiceRequest = ((MaterialDesignThemes.Wpf.PackIcon)(target));
            return;
            case 3:
            this.MessageHeaderServiceRequest = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.bottomHeaderGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.searchGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.requestSearch = ((System.Windows.Controls.TextBox)(target));
            
            #line 62 "..\..\..\Views\ServiceRequestPage1.xaml"
            this.requestSearch.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.requestSearch_TextChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CreateServiceRequestBtn = ((System.Windows.Controls.Button)(target));
            
            #line 72 "..\..\..\Views\ServiceRequestPage1.xaml"
            this.CreateServiceRequestBtn.Click += new System.Windows.RoutedEventHandler(this.CreateServiceRequestBtn_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ServiceRequestsList = ((System.Windows.Controls.ListBox)(target));
            
            #line 80 "..\..\..\Views\ServiceRequestPage1.xaml"
            this.ServiceRequestsList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ServiceRequestsList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

