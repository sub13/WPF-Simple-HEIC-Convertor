   M S B A M L   `   `   `  ����   Simple HEIC convertor&     Simple_HEIC_convertor.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    � Icon$  
apple1.ico��   MaterialDesignBody#���   $��Regular"�$��13(�&   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�   TextRenderingMode$
 Auto=�  MaterialDesignPaper#���    MaterialDesignFont#���   � Title$ Simple HEIC Convertor��$	��480��$	��640�� � 
ResizeMode$ CanMinimize=�.��� $��
0,-2,-8,-4q�$��Right=�$	��640��$	��455��$	��Top=� � RowDefinitions �� $��0* ���  � ColumnDefinitions �� $��0* ��� $��0* ��� $��3* ��� $
��125* �.+��� -   $��Выбрать файлы�� J� 	AllowDrop . $��34,272,361,148q�$��3��$
��Auto��$��1���� -   $#��Путь сохранения��$��344,272,51,148q�$��3��$
��Auto��$��1��	� -    	�Name$ RadioButton1��$	��JPG��$��35,391,542,43q�	 J� RenderTransformOrigin$	 0.14,-1.003&�$��1��$��3��	� -   $ RadioButton2��$	��PNG��$��35,365,542,69q�$	 0.14,-1.003&�$��1��$��3���� -   $"��Конвертировать��$��228,332,256,88q�$
��Auto��$��1��$��3���� $��3��$��
0,0,15,452q�$��2��� -   
 �Name$
 progressBar1��
�������
������$
��Auto��$��39,18,51,397q� � Value$ 0Q� � SmallChange$ 0Q�$��1��$��3���� -    ��Name$ FilesViewer��
������� . $��35,102,51,208q�$	 
-0.32,0.82&���. $��1��$��3��.���� -    ��Name$ 
FilesPanel��$	��142��$	��524�� � ToolTip _� .����Нажмите на файл два раза левой кнопкой для предварительного просмотра!�� $��Формат:��$��34,338,513,90q�$��1��$��3���� $"��Открытые файлы:��$��35,63,281,365q�$��1��$��3���� -	    ��Name$ CleanFilesButton��$y�Hidden=�$��530,64,51,365q�$
��Auto��$��1��$��3��$	 
0.404,0.35&�.����  �� Orientation$ 
Horizontal=�.+�'  !MaterialDesignThemes.Wpf.PackIcon     Kind$ Broom=�$��16��$��14��'  !MaterialDesignThemes.Wpf.PopupBox  $��PopupBox��$��2��$��Stretch=�$��10,25,596,404q�$��2��$��2��   PopupContent. �� -
   $��Справка���� -    ��Name$ ThemeCheckBox��$��Темная тема��$��3��$
��Left=�$��475,394,0,0q�$��2��$	��Top=�$��99��)  #MaterialDesignThemes.Wpf.DialogHost     DialogContent$	 fsa��.��sadfa