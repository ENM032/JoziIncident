   M S B A M L   `   `   `  ����T  PST10091324_PROG7312_Part1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:ST10091324_PROG7312_Part1.ViewsST10091324_PROG7312_Part1.Views  5      *    $ST10091324_PROG7312_Part1.Views.Home   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      9local-clr-namespace:ST10091324_PROG7312_Part1.Views  5        N� Title$
  Home��5
       N� 
Background
 ���� .��5      �  � RowDefinitions 5   
   �� 5      -   5         ��Name$ homeHeaderRowDef��5      $��60 �66   �� 5       � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��2��6.   $��0��6B   $��Center=�6O   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon  -   5          Name$ HomeHeaderIcon��5   &      Kind$ EmojiSad=�5   &   $��30��5   &   $��30��62   
��� ���   menuButtonIcon#��[  5   &   �� 5      -   5         ��Name$ 
homeHeader��5      $��18(�5      $	��700��6&   
��� ���$��WrapWithOverflow=�6J   |zWe didn't find any reported incidents. We would love to get reports from you, please navigate to the report incident page.5!      5"      &   System.Windows.Controls.DataGrid  5+   
   -   5        	  Name$	 CustomerGrid��5+      $��2��6)   $��1��6=   
   AutoGenerateColumns
 .  ���#eff2f7   
IsReadOnly .    Columns 5,      0  *System.Windows.Controls.DataGridTextColumn  5-      ,  &System.Windows.Controls.DataGridColumn   Header$ Incident ID��6%   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6:   �� *Id+  5.      $ Location��6%    67   �� *
Location+  5/      $ Category��6%    67   �� *
Category+  50      $ Description��6%    6:   �� *Description+  51      $ Media File Path��6%    6>   �� *MediaFilePath+53      54      55      