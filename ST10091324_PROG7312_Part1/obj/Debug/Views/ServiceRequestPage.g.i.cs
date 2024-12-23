﻿#pragma checksum "..\..\..\Views\ServiceRequestPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "4FE53F1012695CF467BDC3F14332D5BDA561BE1D0C2BFC9C0C4A29EA76531203"
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
    /// ServiceRequestPage
    /// </summary>
    public partial class ServiceRequestPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 32 "..\..\..\Views\ServiceRequestPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel MessageStackPanelServiceRequest;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\..\Views\ServiceRequestPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.PackIcon MessagetHeaderIconServiceRequest;
        
        #line default
        #line hidden
        
        
        #line 42 "..\..\..\Views\ServiceRequestPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MessageHeaderServiceRequest;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\Views\ServiceRequestPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid bottomHeaderGrid;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\Views\ServiceRequestPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid searchGrid;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\Views\ServiceRequestPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox requestSearch;
        
        #line default
        #line hidden
        
        
        #line 69 "..\..\..\Views\ServiceRequestPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CreateServiceRequestBtn;
        
        #line default
        #line hidden
        
        
        #line 78 "..\..\..\Views\ServiceRequestPage.xaml"
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
            System.Uri resourceLocater = new System.Uri("/ST10091324_PROG7312_Part1;component/views/servicerequestpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ServiceRequestPage.xaml"
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
            
            #line 63 "..\..\..\Views\ServiceRequestPage.xaml"
            this.requestSearch.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.requestSearch_TextChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CreateServiceRequestBtn = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\..\Views\ServiceRequestPage.xaml"
            this.CreateServiceRequestBtn.Click += new System.Windows.RoutedEventHandler(this.CreateServiceRequestBtn_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ServiceRequestsList = ((System.Windows.Controls.ListBox)(target));
            
            #line 81 "..\..\..\Views\ServiceRequestPage.xaml"
            this.ServiceRequestsList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ServiceRequestsList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 9:
            
            #line 106 "..\..\..\Views\ServiceRequestPage.xaml"
            ((System.Windows.Controls.ProgressBar)(target)).Initialized += new System.EventHandler(this.StatusProgressBar_Initialized);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}

