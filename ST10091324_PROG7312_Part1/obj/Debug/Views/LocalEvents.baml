   M S B A M L   `   `   `  ����T  PST10091324_PROG7312_Part1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:ST10091324_PROG7312_Part1.ViewsST10091324_PROG7312_Part1.Views  5      1    +ST10091324_PROG7312_Part1.Views.LocalEvents   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      :d2http://schemas.microsoft.com/expression/blend/2008  5      9local-clr-namespace:ST10091324_PROG7312_Part1.Views  5      8Icon-http://metro.mahapps.com/winfx/xaml/iconpacks  5        N� Title$  LocalEvents��5       N� 
Background �#eff2f7 N� FontSize$ 18(�5       � 	Resources 5      3  -System.Windows.Media.Effects.DropShadowEffect   ShadowEffect   Color   
BlurRadius   ShadowDepth   Opacity��%U   &	          5   
   $ #000000��60   $ 10Q�6@   $ 2Q�6P   $	 0.2Q�6`   5      .��5      � .+�5   
   �� .+�5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� -   5        $��CategoryComboBox��5      $	��200��6h   )  #MaterialDesignThemes.Wpf.HintAssist	   Hint$	 Filter by category��5       " MaterialDesignOutlinedComboBox#��[ 5      (  "System.Windows.Controls.DatePicker  5      -   5        $��StartDatePicker��5      $��Center=�5       $��0q�6;    ( $MaterialDesignFloatingHintDatePicker#��[ 5!      $	 From:��5"      $��10q�6@     5&      -   5        $��EndDatePicker��5&      $��Center=�5'      $��0q�6;   #��[ 5(      $		 To:��5)      $��10q�6>   �� 5-      -   5        $��ResetFiltersButton��5-      $��Reset Filters��63   $��10,7,0,0q�5.        MaterialDesignPaperButton#��[ 6'   50      �� 54      -   5        
 ��Name$
 UpcomingEventsHeader��54      $%��Upcoming Events & Announcements��66   $
��Bold"�6a   $��20(�6s   $��10 0 0 0q�6�   �� 58      -   5         ��Name$ EventScrollViewer��58      $
��Auto=�66   $	��250��6Y   $��10q�6f   .��59      �� -   5        $��
EventsList��59       %� IsVirtualizing .  %� VirtualizationMode$ 	Recycling=�6^   ��5:      �� 5;      .R�5=      �� $��0 0 0 10q�6%    �� Padding$ 10q�67   
������� �� CornerRadius$ 8��6W    J� Effect# [  5>   !   ���#E0E0E0$��1q�6^   ._�5?   "   �� $ 
Horizontal=�6-   .+�5A   &   �� $	��100��6H   $	��100��6U   $��Uniform=�6a   $��0,0,10,0q�6s   ��6,   �� *
ImageUrl+6s   �� 5D   &   .+�5F   *   �� $
��Bold"�64   ��6F   �� *Title+�� 5J   *   $��SemiBold"�64   $��15(�6d   ��6J   �� *
Category+6d   �� 5N   *   $
��Wrap=�6Q   $��14(�6e   ���#667$	��500��6�   ��64   �� *Description+6�   �� 5R   *   $��16(�6n   ��64   �� *Date+ �� StringFormat$ {0:MMMM dd, yyyy}��6n   5T   '   5U   #   5V      5X      5Z      5[      �� 5a      -   5        $
 RecommendationsHeader��5a      $(��"Recommended Events & Announcements��67   $
��Bold"�6e   $��20(�6w   $��	10 10 0 5q�6�   y�5b      �� *IsRecommendationsVisible+   BooleanToVisibilityConverter �� 	Converter# [ �� 5f      -	   5        $! RecommendationsScrollViewer��5f      $
��Auto=�6@   $	��150��6c   $��10q�6p   y�5g      �� *IsRecommendationsVisible+# [ .��5h      �� -
   5        $��RecommendationsList��5h       . $ 	Recycling=�6g   ��5i      �� 5j      .R�5l      �� $��0 0 0 10q�6%   $ 10q�67   
�������$ 8��6W   # [  5m   !   ���#E0E0E0$��1q�6^   ._�5n   "   �� $ 
Horizontal=�6-   .+�5p   &   �� $	��100��6H   $	��100��6U   $��Uniform=�6a   $��0,0,10,0q�6s   ��6,   �� *
ImageUrl+6s   �� 5s   &   .+�5u   *   �� $
��Bold"�64   ��6F   �� *Title+�� 5y   *   $��SemiBold"�64   $��15(�6d   ��6J   �� *
Category+6d   �� 5}   *   $
��Wrap=�6Q   $��14(�6e   ���#667$	��500��6�   ��64   �� *Description+6�   �� 5�   *   $��16(�6n   ��64   �� *Date+$ {0:MMMM dd, yyyy}��6n   5�   '   5�   #   5�      5�      5�      5�      5�      5�      5�      