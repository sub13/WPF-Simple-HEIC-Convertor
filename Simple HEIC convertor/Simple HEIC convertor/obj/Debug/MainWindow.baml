   M S B A M L   `   `   `  ����  Simple HEIC convertor&     Simple_HEIC_convertor.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Icon$  
apple1.ico��5   	      MaterialDesignBody#���   5	   	   $��Regular"�5
   	   $��13(�5   	   &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5   	      TextRenderingMode$
 Auto=�5   	     MaterialDesignPaper#���  5   	     MaterialDesignFont#���  5   	    � Title$ Simple HEIC Convertor��5   	   $	��480��6'   $	��640��64    � 
ResizeMode$ CanMinimize=�6@   .��5      � $��
0,-2,-8,-4q�6   $��Right=�6   $	��640��6;   $	��455��6G   $	��Top=�6T    � RowDefinitions 5   
   �� 5      $��0* �6   �� 5       � ColumnDefinitions 5   
   �� 5      $��0* �6   �� 5      $��0* �6   �� 5      $��3* �6   �� 5      $
��125* �6   .+�5   
   �� -   5        $��Выбрать файлы��5       J� 	AllowDrop . $��34,272,361,148q�6`   $��3��6x   $
��Auto��6�   $��1��6�   �� 5   
   -   5        $#��Путь сохранения��5      $��344,272,51,148q�6+   $��3��6C   $
��Auto��6j   $��1��6x   	� 5    
   -   5         	�Name$ RadioButton1��5       $	��JPG��6,   $��35,391,542,43q�6:   	 J� RenderTransformOrigin$	 0.14,-1.003&�6Q   $��1��6u   $��3��6�   	� 5!   
   -   5        $ RadioButton2��5!      $	��PNG��6-   $��35,365,542,69q�6;   $	 0.14,-1.003&�6R   $��1��6v   $��3��6�   �� 5"   
   -   5        $"��Конвертировать��5"      $��228,332,256,88q�6*   $
��Auto��6Y   $��1��6g   $��3��6t   �� 5#   
   $��3��6   $��
0,0,15,452q�6%   $��2��69   6L   � 5$   
   -   5        
 �Name$
 progressBar1��5$   @   
�������
������$
��Auto��6V   $��39,18,51,397q�6d    � Value$ 0Q�6z    � SmallChange$ 0Q�6�   $��1��6�   $��3��6�   �� 5%   
   -   5         ��Name$ FilesViewer��5%   *   
������� . $��35,102,51,208q�6w   $	 
-0.32,0.82&�6�   ��. $��1��6�   $��3��6�   .��5&      �� -   5         ��Name$ 
FilesPanel��5&      $	��142��6.   $	��524��6;    � ToolTip 5'      _� 5(      .��6   ��Нажмите на файл два раза левой кнопкой для предварительного просмотра!5*      5,      5-      �� 5.   
   $��Формат:��6   $��34,338,513,90q�6"   $��1��69   $��3��6F   �� 5/   
   $"��Открытые файлы:��6   $��35,63,281,365q�6*   $��1��6A   $��3��6N   �� 50   
   -	   5         ��Name$ CleanFilesButton��50   9   $y�Hidden=�6   $��530,64,51,365q�6S   $
��Auto��6j   $��1��6x   $��3��6�   $	 
0.404,0.35&�6�   .��51      ��  �� Orientation$ 
Horizontal=�6   .+�52      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Broom=�6*   $��16��67   $��14��6C   53      54      '  !MaterialDesignThemes.Wpf.PopupBox  56   
   $��PopupBox��6"   $��2��65   $��Stretch=�6E   $��10,25,596,404q�6c   $��2��6z   $��2��6�      PopupContent. 57      �� -
   5        $��Справка��57      6'   58      �� 59   
   -   5         ��Name$ ThemeCheckBox��59      $��Темная тема��6*   $��3��6@   $
��Left=�6P   $��475,394,0,0q�6k   $��2��6�   $	��Top=�6�   $��99��6�   )  #MaterialDesignThemes.Wpf.DialogHost  5:   
      DialogContent$	 fsa��5=   7   .��sadfa5?      5@      5A      