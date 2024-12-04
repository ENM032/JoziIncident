﻿#pragma checksum "..\..\..\Views\SignUp.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F053546F13DCB34B497F8AE87F5AD86F9B86F35AB62DDE5DC8AE1F2292EFFB09"
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
    /// SignUp
    /// </summary>
    public partial class SignUp : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 57 "..\..\..\Views\SignUp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RowDefinition RowDefinitionForm2;
        
        #line default
        #line hidden
        
        
        #line 61 "..\..\..\Views\SignUp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button MinimiseBtn;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\Views\SignUp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseBtn;
        
        #line default
        #line hidden
        
        
        #line 92 "..\..\..\Views\SignUp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock MessageTxtBlock;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\Views\SignUp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox UsernameTxtBox;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\Views\SignUp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox EmailTxtBox;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\Views\SignUp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PasswordBox;
        
        #line default
        #line hidden
        
        
        #line 103 "..\..\..\Views\SignUp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox ConfirmPasswordBox;
        
        #line default
        #line hidden
        
        
        #line 106 "..\..\..\Views\SignUp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox AgreementCheckBox;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\..\Views\SignUp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button BtnSignUp;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\Views\SignUp.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock LoginLinkText;
        
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
            System.Uri resourceLocater = new System.Uri("/ST10091324_PROG7312_Part1;component/views/signup.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Views\SignUp.xaml"
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
            this.RowDefinitionForm2 = ((System.Windows.Controls.RowDefinition)(target));
            return;
            case 2:
            this.MinimiseBtn = ((System.Windows.Controls.Button)(target));
            
            #line 62 "..\..\..\Views\SignUp.xaml"
            this.MinimiseBtn.Click += new System.Windows.RoutedEventHandler(this.MinimiseBtn_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.CloseBtn = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\..\Views\SignUp.xaml"
            this.CloseBtn.Click += new System.Windows.RoutedEventHandler(this.CloseBtn_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.MessageTxtBlock = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.UsernameTxtBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 95 "..\..\..\Views\SignUp.xaml"
            this.UsernameTxtBox.KeyUp += new System.Windows.Input.KeyEventHandler(this.UsernameTxtBox_KeyUp);
            
            #line default
            #line hidden
            return;
            case 6:
            this.EmailTxtBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 98 "..\..\..\Views\SignUp.xaml"
            this.EmailTxtBox.LostFocus += new System.Windows.RoutedEventHandler(this.EmailTxtBox_LostFocus);
            
            #line default
            #line hidden
            
            #line 98 "..\..\..\Views\SignUp.xaml"
            this.EmailTxtBox.KeyUp += new System.Windows.Input.KeyEventHandler(this.EmailTxtBox_KeyUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.PasswordBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 101 "..\..\..\Views\SignUp.xaml"
            this.PasswordBox.KeyUp += new System.Windows.Input.KeyEventHandler(this.PasswordBox_KeyUp);
            
            #line default
            #line hidden
            return;
            case 8:
            this.ConfirmPasswordBox = ((System.Windows.Controls.PasswordBox)(target));
            
            #line 104 "..\..\..\Views\SignUp.xaml"
            this.ConfirmPasswordBox.KeyUp += new System.Windows.Input.KeyEventHandler(this.ConfirmPasswordBox_KeyUp);
            
            #line default
            #line hidden
            return;
            case 9:
            this.AgreementCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 106 "..\..\..\Views\SignUp.xaml"
            this.AgreementCheckBox.KeyUp += new System.Windows.Input.KeyEventHandler(this.AgreementCheckBox_KeyUp);
            
            #line default
            #line hidden
            return;
            case 10:
            this.BtnSignUp = ((System.Windows.Controls.Button)(target));
            
            #line 112 "..\..\..\Views\SignUp.xaml"
            this.BtnSignUp.Click += new System.Windows.RoutedEventHandler(this.BtnSignUp_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.LoginLinkText = ((System.Windows.Controls.TextBlock)(target));
            
            #line 116 "..\..\..\Views\SignUp.xaml"
            this.LoginLinkText.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.LoginLinkText_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
