﻿#pragma checksum "C:\Work\Visual Studio Project\Match Animal\Match Animal\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F34308AE2DA2CC2B013FE55F34D6676C"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Match_Animal
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    global::Windows.UI.Xaml.Controls.Page element1 = (global::Windows.UI.Xaml.Controls.Page)(target);
                    #line 8 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Page)element1).SizeChanged += this.Page_SizeChanged;
                    #line default
                }
                break;
            case 2:
                {
                    this.grid_container = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3:
                {
                    this.gridView_container = (global::Windows.UI.Xaml.Controls.GridView)(target);
                }
                break;
            case 4:
                {
                    this.progress_bar = (global::Windows.UI.Xaml.Controls.ProgressBar)(target);
                }
                break;
            case 5:
                {
                    this.btn_print = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 16 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_print).Click += this.btn_print_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

