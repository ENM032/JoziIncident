   M S B A M L   `   `   `  ����T  PST10091324_PROG7312_Part1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:ST10091324_PROG7312_Part1.ViewsST10091324_PROG7312_Part1.Views  5      Y1clr-namespace:ST10091324_PROG7312_Part1.Converter#ST10091324_PROG7312_Part1.Converter  5      9    3ST10091324_PROG7312_Part1.Views.ServiceRequestPage1   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ?local-clr-namespace:ST10091324_PROG7312_Part1.Views     5      _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      B
converters1clr-namespace:ST10091324_PROG7312_Part1.Converter  5        N� Title$  ServiceRequest��5       N� 
Background �#eff2f7 N� FontSize$ 18(�5       � 	Resources 5      @   :ST10091324_PROG7312_Part1.Converter.StatusToColorConverter   StatusToColorConverterC   =ST10091324_PROG7312_Part1.Converter.ProgressBarColorConverter  ProgressBarColorConverter�� 5   
   %:   &	        &	        5      60     5      63   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   �� -   5        $%��MessageStackPanelServiceRequest��5       �� Orientation$ 
Horizontal=�6<   $��2��5       $��0��6%   $��0,0,0,5q�62   .+�5"      '  !MaterialDesignThemes.Wpf.PackIcon  -   5          Name$&  MessagetHeaderIconServiceRequest��5"   &      Kind$ EmoticonConfused=�5#   "   $��40��5$   "   $��40��6-   $��Center=�5%   "   $��Right=�6=   
��� ���$��0,0,15,0q�5'   "   �� 5)      -   5         ��Name$! MessageHeaderServiceRequest��5)      $��18(�5*      $	��700��6"   
��� ���$��WrapWithOverflow=�6F   ��We didn't find any service requests for your account. Click on the create service request button below to open a service request.5,      5-      � 50   
   -   5        	 �Name$	 bottomHeaderGrid��50      $��1��6)   $��Center=�66   $��Stretch=�6Q   
 � ColumnDefinitions
 52      �� 53      $
��Auto �6#   �� 55      $��* �6#   �� 57      $
��Auto �6#   .+�5<      � -   5        $	 
searchGrid��5<      $	��300��6'   $
��Left=�63   $��Center=�6N   $��1��6i   .+�5=      �� -   5         ��Name$ requestSearch��5=        requestBoxSearch#��[ 61   $��0q�5>      $��15 10q�6"   62     5?      $ Magnify=�6*   $��20��69   $��20��6D   $��Center=�6P   $��Right=�6k   $��0,0,15,0q�5@      ���#b0b9c660   5A      �� 5D      -   5         ��Name$ CreateServiceRequestBtn��5D      $��Create Service Request��66   $��15(�5E      $��Right=�5F      $	��200��5G      $��30��6    $��0��5H      $��0,0,20,0q�5I      5J      �� 5M   
   -   5        $��ServiceRequestsList��5M      
������ $��2��6F   ��.  %� IsVirtualizing .  %� VirtualizationMode$ 	Recycling=�5P   	   ��5Q      �� 5R      .R�5S      #  MaterialDesignThemes.Wpf.Card  $��20q�6*   $��0,0,0,15q�67   ��6I   ��  �� RelativeSource ��  �� AncestorType$ ListBoxL� �� Path$ ActualWidth�.��5T      �� .+�5U      �� $��16(�6C   $
��Bold"�6Q   ���#212121��6(   �� *	RequestID+6c   �� 5V      $��0,5,0,15q�6E   ���#757575��6(   �� *Description+6W   �� 5Y      $��Status: ��6(   ���#616161$
��Bold"�6M   �� 5\      .+�5^   "   �� $
��Bold"�5`      ��5^   ,   �� *Status+��5_      �� *Status+ �� 	Converter# [  5`      5c      � 5f       �Name$ StatusProgressBar��6*    � Minimum$ 0Q�6`    � Maximum$	 100Q�6l   $��20��6z   $��	0,15,30,0q�5h   *    � Value 5f   E   �� *
Progress+��5g   *   ��  �� $ materialDesign:CardL�$ ActualWidth� � IsIndeterminate 5h   =   �� *IsIndeterminate+��5i   *   �� *Status+# [ �� 5l      $��0,10,0,0q�6(   $
��Bold"�6:   $��12(�6L   ��5m   "   �� 5n   &    �� 
TargetType" ��6,    �� Triggers 5o   *   �� 5q   .    �� Binding 6:   �� *SearchRequestType+ �� Value S6`   .a�5r   2   ��  �� Property � �� �� Value$ Search Result��6I   �� 5s   2    � ��
 � � �6O   5t   /   �� 5v   .    6:   �� *SearchRequestType+ R6`   .a�5w   2   ��  � ��$ Related Request��6I   �� 5x   2    � ��
 �����6O   5y   /   5{   '   5}      5      5�      5�      5�      5�      5�      