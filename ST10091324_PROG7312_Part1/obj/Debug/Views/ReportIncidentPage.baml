   M S B A M L   `   `   `  ����T  PST10091324_PROG7312_Part1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:ST10091324_PROG7312_Part1.ViewsST10091324_PROG7312_Part1.Views  5      8    2ST10091324_PROG7312_Part1.Views.ReportIncidentPage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:ST10091324_PROG7312_Part1.Views     5      _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        N� Title$  ReportIncidentPage��5
       N� 
Background �#eff2f7 N� FontSize$ 18(�5      .��5      �  � ColumnDefinitions 5   
   �� 5      $	��250 �6   �� 5      $
��auto �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0��6.   $��2��6>   $	��Top=�6R   $��0 3 0 0q�6j   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon  -   5          Name$ MessageIcon��5   &      Kind$ HandClap=�5   &   
��� � �   menuButtonIcon#��[  5   &   $��20��5   &   $��20��62   �� 5      -   5         ��Name$ MessageTxtBlock��5      
��� � �$��	-13 2 0 0q�6E   CAThank you for choosing to make the city of Joburg a better place!5      5      �� 5    
   $��0��6   $ Vertical=�6%   $
��0 35q�6<   .+�5!      �� $��	Location:��6   $
��Left=�6(   $��	70 0 0 30q�6C   �� 5#      $��	Category:��6   $
��Left=�6(   $��	70 0 0 30q�6C   �� 5%      $��Description:��6   $
��Left=�6+   $��	70 0 0 30q�6F   �� 5'      $��Upload image:��6   $
��Left=�6,   $��
70 60 0 30q�6G   5(      �� 5+   
   $��1��6   $ Vertical=�6%   $
��0 35q�6<   .+�5-      �� -   5         ��Name$ LocationTxtBx��5-        textBoxForm#��[ 6-   6n   � 5/      -   5        	 �Name$	 AutoCompletorListPopup��5/      $y�	Collapsed=�50      
 � 	StaysOpen
 .   � AllowsTransparency . $	��150��54      $��Bottom=�55       � PlacementTarget 53      ��  �� ElementName$ LocationTxtBx��.��56      �� -   5         ��Name$ AutoCompletorList��56      ���#eff2f7$	��200��58      $
��20 0q�59        
lstBoxForm#��[ 6^   5:      �� 5<      -   5         ��Name$ CategoryCmbBx��5<      $��40��6.   $	��300��6=   ���#e0e0e0$��0 0 0 30q�6b   �� 5>      -   5         ��Name$ DescriptionRichTxtBx��5>       % !MaterialDesignOutlinedRichTextBox#��[ 68   ���#e0e0e0$��0 0 0 20q�5?      $	��300��6,   $	��100��68   �� 5A      -   5         ��Name$ UploaderBtn��5A      $��0 0 0 20q�6*   $��18(�6V   .��5B      �� $ 
Horizontal=�6   .+�5C        $ 
Multimedia=�5D   &   #��[  5E   &   �� 5F      $��Select Media��6    5G      5H      �� 5J      -	   5        $ SubmitIncidentBtn��5J      $��Submit��60   $��18(�6A   $��Center=�6O   $	��150��6l   6{   5L      5N      5O      