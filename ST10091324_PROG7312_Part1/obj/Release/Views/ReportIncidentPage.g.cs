﻿#pragma checksum "..\..\..\Views\ReportIncidentPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "B4B34586C6BA644EF25F2034C720FA381B8BDD124C55C75A9958959CEFD76E41"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using MahApps.Metro.IconPacks.Converter;
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
    /// ReportIncidentPage
    /// </summary>
    public partial class ReportIncidentPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\Views\ReportIncidentPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MahApps.Metro.IconPacks.PackIconMaterial MessageIcon;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Views\ReportIncidentPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MessageTxtBlock;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\Views\ReportIncidentPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox LocationTxtBx;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\Views\ReportIncidentPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Popup AutoCompletorListPopup;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\Views\ReportIncidentPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox AutoCompletorList;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\Views\ReportIncidentPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox CategoryCmbBx;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\Views\ReportIncidentPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RichTextBox DescriptionRichTxtBx;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\Views\ReportIncidentPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button UploaderBtn;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\Views\ReportIncidentPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button SubmitIncidentBtn;
        
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
            System.Uri resourceLocater = new System.Uri("/ST10091324_PROG7312_Part1;component/views/reportincidentpage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\ReportIncidentPage.xaml"
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
            this.MessageIcon = ((MahApps.Metro.IconPacks.PackIconMaterial)(target));
            return;
            case 2:
            this.MessageTxtBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.LocationTxtBx = ((System.Windows.Controls.TextBox)(target));
            
            #line 41 "..\..\..\Views\ReportIncidentPage.xaml"
            this.LocationTxtBx.KeyUp += new System.Windows.Input.KeyEventHandler(this.LocationTxtBx_KeyUp);
            
            #line default
            #line hidden
            
            #line 41 "..\..\..\Views\ReportIncidentPage.xaml"
            this.LocationTxtBx.LostFocus += new System.Windows.RoutedEventHandler(this.LocationTxtBx_LostFocus);
            
            #line default
            #line hidden
            return;
            case 4:
            this.AutoCompletorListPopup = ((System.Windows.Controls.Primitives.Popup)(target));
            return;
            case 5:
            this.AutoCompletorList = ((System.Windows.Controls.ListBox)(target));
            
            #line 53 "..\..\..\Views\ReportIncidentPage.xaml"
            this.AutoCompletorList.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.AutoCompletorList_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.CategoryCmbBx = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.DescriptionRichTxtBx = ((System.Windows.Controls.RichTextBox)(target));
            return;
            case 8:
            this.UploaderBtn = ((System.Windows.Controls.Button)(target));
            
            #line 61 "..\..\..\Views\ReportIncidentPage.xaml"
            this.UploaderBtn.Click += new System.Windows.RoutedEventHandler(this.UploaderBtn_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.SubmitIncidentBtn = ((System.Windows.Controls.Button)(target));
            
            #line 68 "..\..\..\Views\ReportIncidentPage.xaml"
            this.SubmitIncidentBtn.Click += new System.Windows.RoutedEventHandler(this.SubmitIncidentBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

