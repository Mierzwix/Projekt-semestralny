   M S B A M L   `   `   `  ����D  @GuildInfo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:GuildInfo.ViewsGuildInfo.Views  5   	   ;"clr-namespace:GuildInfo.ViewModelsGuildInfo.ViewModels  5   	   #    GuildInfo.Views.MembersWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:GuildInfo.Views     5   	   1vm"clr-namespace:GuildInfo.ViewModels     5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5	   	   -   5        $��membersWindow��5   7     � WindowStartupLocation$  CenterScreen=�5       � 
ResizeMode$ NoResize=�6@    � Title$ Członkowie gildii��5
   	   $	��450��6#   $	��800��60   .  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *	LoadedCmd+5       � DataContext 5      1   +GuildInfo.ViewModels.MembersWindowViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  	   SelectionMode$	 Single=�6   
   
IsReadOnly
 .    AutoGenerateColumns .     CanUserAddRows .     CanUserDeleteRows .  ��6*   �� *	Members+ �� SelectedItem 6J   �� *SelectedMember+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ Ksywka��6%   1	  +System.Windows.Controls.DataGridBoundColumn 	  Binding 65   �� *
Nickname+  5      $ Poziom��6%    65   �� *Level+  5      $ Profesja��6%    67   �� *Profession.Name+  5      $
 Rasa��6%    63   �� *	Race.Name+  5       $ Gildia��6%    65   �� *
Guild.Name+5"      � 5$   
   $��1��6    � ColumnDefinitions 5%      �� 5&      $
��Auto �6#   �� 5'      $��* �6#   $	��150��6-   �� 5(      $
��Auto �6#   �� 5)      $��* �6#    5+      �� 5,      $
��Auto �6    �� 5-      $
��Auto �6    .+�5/      �� $��Ksywka��6   $��Center=�6&   $��3q�6A   �� 50      $��Center=�6   $��3q�61   $��1��6<   ��6L   �� *TempMember.Nickname+ �� UpdateSourceTrigger$ PropertyChanged=��� 51      $��Poziom��6   $��1��6&   $��Center=�63   $��3q�6N   �� 53      $��Center=�6   $��1��61   $��3q�6>   $��1��6I   ��6Y   �� *TempMember.Level+�� 55      $��2��6   $
��Rasa��6(   $��Center=�64   $��3q�6O   �� 56      $��4��6   $��Center=�6'   $��3q�6B    �� DisplayMemberPath$
 Name��6�   ��6M   �� *Races+ 6k   �� *TempMember.Race+6�   �� 58      $��Profesja��6   $��2��6(   $��1��68   $��Center=�6E   $��3q�6`   �� 59      $��Center=�6   $��1��62   $��3q�6?   $��3��6J   $
 Name��6�   ��6Z   �� *Professions+ 6~   �� *TempMember.Profession+6�   5;      �� 5=   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Center=�6;   .+�5>      �� $	��100��6   $��3q�6!   $��Center=�6,   $
��Nowy��6G   ��6V   �� *NewMemberCmd+�� 5?      $	��100��6   $��3q�6!   $��Center=�6,   $��Dodaj��6G   ��6W   �� *AddMemberCmd+�� 5@      $	��100��6   $��3q�6!   $��Center=�6,   $��Usuń��6G   ��6V   �� *RemoveMemberCmd+�� 5A      $	��100��6   $��3q�6!   $��Center=�6,   $��Zapisz��6G   ��6X   �� *UpdateMemberCmd+�� 5B      $	��100��6   $��3q�6!   $��Center=�6,   $��Zamknij��6G   ��6Y   �� *
CloseCmd+��6v   ��  �� ElementName$ membersWindow��5C      5D      5E      