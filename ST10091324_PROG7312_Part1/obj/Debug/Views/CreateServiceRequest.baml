   M S B A M L   `   `   `  ����T  PST10091324_PROG7312_Part1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:ST10091324_PROG7312_Part1.ViewsST10091324_PROG7312_Part1.Views  5      :    4ST10091324_PROG7312_Part1.Views.CreateServiceRequest   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:ST10091324_PROG7312_Part1.Views     5      _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5        N� Title$  CreateServiceRequest��5
       N� 
Background �#eff2f7 N� FontSize$ 18(�5      .��5      � .+�5   
   �� .+�5      '  !MaterialDesignThemes.Wpf.PackIcon  -   5        $��
BackButton��5   &      Kind$ ArrowBackCircle=�68   $��50��5   &   $��50��61   ���#b0b9c65   ;   �� 5      $��Description:��6    %  !MaterialDesignTitleLargeTextBlock#��[  5      $��50,0,0,5q�5      $
��Left=�5      �� 5      -   5         ��Name$" DescriptionServiceRequestRTB��5       % !MaterialDesignOutlinedRichTextBox#��[ 6@   ���#e0e0e0$��	50,0,0,20q�5   0   $	��300��5      $	��400��6)   $	��100��5      $	��200��6*   $
��Left=�5      �� 5"      $��	Location:��6   #��[  5#      $��	50,20,0,5q�5$      $
��Left=�5%      �� 5'      -   5         ��Name$ LocationTxtBx��5'        textBoxForm#��[ 5(      $
��Left=�5)      $��30��5*      $��50��6%   $	��400��5+      $	��600��6%   $��	50,0,0,20q�5,      � 5.      -   5         �Name$ AutoCompletorListPopup��5.      $y�	Collapsed=�5/       � 	StaysOpen .   � AllowsTransparency . $	��150��53      $��Bottom=�54      	 � PlacementTarget	 52      �� 
 �� ElementName$
 LocationTxtBx��.��55      �� -   5         ��Name$ AutoCompletorList��55      ���#eff2f7$	��200��57      $
��20 0q�58        
lstBoxForm#��[ 59      5:      �� 5=      -   5        $��Create Service Request��5=      $
��Left=�5>      $��40��5?      $��60��6$   $	��300��5@      $	��600��6%   $��
100,20,0,0q�5A      $��18(�5B      6#   �� 5D      $����Service request completion dates are just estimates and may take longer than the stipulated date to resolve, especially during the holiday season.��6   $
��Wrap=�5E      $
��Left=�5F      $	��300��5G      $	��500��6'   $��	50,20,0,0q�5H      
�������$��16(�5J      5K      5L      5M      