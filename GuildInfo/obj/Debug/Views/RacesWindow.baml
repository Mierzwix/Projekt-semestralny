   M S B A M L   `   `   `  ����D  @GuildInfo, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:GuildInfo.ViewsGuildInfo.Views  5   	   ;"clr-namespace:GuildInfo.ViewModelsGuildInfo.ViewModels  5
   	   !    GuildInfo.Views.RacesWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:GuildInfo.Views     5   	   c _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5	   	   +vm"clr-namespace:GuildInfo.ViewModels  5
   	   -   5        $��racesWindow��5   	     � WindowStartupLocation$  CenterScreen=�5       � 
ResizeMode$ NoResize=�6?    � Title$
 Rasy��5   	   $	��450��6   $��334.705��6#   .  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Loaded��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *	LoadedCmd+5       � DataContext 5      /   )GuildInfo.ViewModels.RacesWindowViewModel  5   
   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   �� 	 �� DisplayMemberPath$
	 Name��6V   ��6   �� *Races+
 �� SelectedItem
 60   �� *SelectedRace+6V   � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5       $��* �6#   $	��150��6-    5"      �� 5#      $
��Auto �6    �� 5$      $
��Auto �6    .+�5&      �� $��Nazwa��6   $��Center=�6%   $��3q�6@   �� 5'      $��Center=�6   $��3q�61   $��1��6<   ��6L   �� *TempRace.Name+ �� UpdateSourceTrigger$ PropertyChanged=�5(      � 5*   
   $��2��6    � Orientation$ 
Horizontal=�6!   $��Center=�6:   .+�5+      �� $	��100��6   $��3q�6!   $��Center=�6,   $
��Nowy��6G   ��6V   �� *
NewRaceCmd+�� 5,      $	��100��6   $��3q�6!   $��Center=�6,   $��Dodaj��6G   ��6W   �� *
AddRaceCmd+�� 5-      $	��100��6   $��3q�6!   $��Center=�6,   $��Usuń��6G   ��6V   �� *RemoveRaceCmd+�� 5.      $	��100��6   $��3q�6!   $��Center=�6,   $��Zapisz��6G   ��6X   �� *UpdateRaceCmd+�� 5/      $	��100��6   $��3q�6!   $��Center=�6,   $��Zamknij��6G   ��6Y   �� *
CloseCmd+��6v   ��  �� ElementName$ racesWindow��50      51      52      