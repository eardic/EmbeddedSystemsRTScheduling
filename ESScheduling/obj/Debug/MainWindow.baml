   M S B A M L   `   `   `  ����G  CESScheduling, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ESSchedulingESScheduling  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	       ESScheduling.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:ESScheduling     5   	   R NOxyPlot.Wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=75e952ba404cdbb0 oxyhttp://oxyplot.org/wpf  5   	   3System&clr-namespace:System;assembly=mscorlib  5   	     � WindowStartupLocation$  CenterScreen=�5	       � Title$2 ,Realtime Scheduling Algorithms [Emre ARDIÇ]��5   	   $	��600��6=   $
��1000��6J    � DataContext 5
   	   ��  �� RelativeSource �� *self+.��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��0.4* �6   �� 5      $
��Auto �6   �� 5      $
��0.6* �6   .+�5   
   �� -   5        $��Status��5      $��0��6"   $
��Bold"�6/    �� Padding$ 0q�6A   $��16(�6M   $��Center=�6[   6z   �� 5   
   $��1��6   .+�5      �� ��5      �� 5      $��Schedule Parameters��6    $
��10pt(�6;   $
��Bold"�6K   6_   .��5      � .+�5      �� -   5         ��Name$ BtnLoadJobs��5      $��	Load Jobs��62   $��Center=�6F   $��75��6a   $��30��6l   $��10q�6x   $
��Bold"�6�   ��5       �� 5!       �� EndPoint$ 0.5,1&�62    �� 
StartPoint$ 0.5,0&�6C   .��5"   "   � 	 � Color$	 Black��6/   
 � Offset$
 0Q�6=   � 5#   "   $	 	#FFC4CEFF��6/   $
 1Q�6A   � 5$   "   $	 	#FFDBE4FF��6/   $
 0.013Q�6A   � 5%   "   $	 	#FF030406��6/   5&      5(      �� 5)      $��	Algorithm��6   $
��Bold"�60   $��Center=�6B   �� 5*      -   5        $��CbSchedules��5*   C   $��10q�6   $	��100��6+   $��23��67    �� 
IsEditable .  {�.  .@�5+      ��EDD��6-   ��5,      EDF��6-   ��5-      RM��6,   ��5.      LLF��6-   5/      �� 50      $��Runtime��6   $
��Bold"�6.   $��Center=�6@   �� 51      -   5         ��Name$ 
TxtRunTime��51      $��Center=�62    �� TextWrapping$
 Wrap=�6M   $��75��6a   ��6l   �� *RunTimeOfScheduling+�� 52      -   5        $ BtnRunSchedule��52      $��Run Schedule��65   $��Center=�6L   $��85��6g   $��30��6r   $��10q�6~   {�.  $
��Bold"�6�   ��53      �� 54      $ 0.5,1&�62   $ 0.5,0&�6C   .��55   "   � $	 Black��6/   $
 0Q�6=   � 56   "   $	 	#FFC4CEFF��6/   $
 1Q�6A   � 57   "   $	 	#FFDBE4FF��6/   $
 0.013Q�6A   � 58   "   $	 	#FF030406��6/   59      5;      �� 5<      -   5        $��LblSchedule��5<   ;   $��Status : Not Runned��6    $��16(�6N   $
��Bold"�6\   $��Center=�6n   
���  ��6�   5=      5>      �� 5@      ��5A      �� 5B      $"��Periodic Schedulability Test��6    $
��10pt(�6D   $
��Bold"�6T    � 	Resources 5D       �� 
TargetType �� Property �� Value��%W   '��       �� 5E      " ��6   .�5F      ��  � ��$ 16(�65   6B   5G      5H      .��5I      � .+�5J      �� $��Images/schedulability.PNG��6   $��50��6?   �� 5K      $��,��6    $��Center=�6)   $
��Bold"�6D   $��5,0,5,0q�6V   �� 5L      -   5         ��Name$ TxtSchedulabilityValue��5L       $��Center=�6@   $
��Bold"�6[   �� 5M      -   5        $ TxtSchedulability��5M       $��Center=�6;   $
��Bold"�6V   $��5,0,0,0q�6h   5N      5O      5P      �� 5S   
   $��2��6   ��5T      �� 5U      $��	Job Table��6   $
��10pt(�6-   $
��Bold"�6=   .��5W      &   System.Windows.Controls.DataGrid  -	   5        $��jobTable��5W         AutoGenerateColumns .     
IsReadOnly . ��6'   �� *Jobs+   Columns 5X      0  *System.Windows.Controls.DataGridTextColumn  5Y      ,  &System.Windows.Controls.DataGridColumn   Header$ Id��6)      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6L   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *Id+6X     5Z      $ Arrival Time��6)   $ * 6_    6?   �� *ArrivalTime+6k     5[      $ Computation Time��6)   $ * 6g    6C   �� *ComputationTime+6s     5\      $ Deadline��6)   $ * 6X    6;   �� *
Deadline+6d     5]      $ Period��6)   $ * 6T    69   �� *Period+6`     5^      $
 Type��6)   $ * 6P    67   �� *Type+6\   5`      5a      �� 5b   
   $��3��6   $��Stretch=�6$   $��5��6B   
�������$��0,3,0,3q�6d   �� 5c   
   $��4��6   ��5d      �� 5e      $��Schedule Graph��6   $
��10pt(�62   $
��Bold"�6B   .��5g        OxyPlot.Wpf.PlotView  -
   5        $��SchedulePlot��5g      5h      5i      5j      5k      