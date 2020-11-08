# KeyboardMapper

Позволяет изменить коды клавиш или отключить их.

При нажатии клавиши проверяет, задана ли она в файле *Mapper.cfg*. 
Если да, то перехватывает событие. Если задана заменяющая 
последовательность, эмулирует ее нажатие.

Для работы программы не требуются права администратора.
Минимальный набор файлов: .exe и Mappings.cfg.

## Установка

1. Запустить программу. Появится иконка в трее.
1. Нажимая нужные клавиши, получить из всплывающих окон их имена.
1. Остановить программу из контекстного меню значка в трее.
1. Заполнить *Mappings.cfg* в папке с программой.
1. Поставить программу в автозагрузку.

## Формат *Mapper.cfg*:

**Внимание!** Если файл пуст, перехватываются ВСЕ нажимаемые 
клавиши, и их имена отображаются во всплывающем окне.

При данных настройках клавиша отправки письма будет вести себя как
Esc, F1 вовсе не будет работать, а при нажатии F2 последовательно
нажмутся *1, 2, 3 и Enter*.

~~~
**Имя_Клавиши1**=**Заменяющая_последовательность1**
**Имя_Клавиши2**=**Заменяющая_последовательность2**

LaunchMail={Esc}
F1=
F2=123{Enter}
~~~

Имена клавиш - значения [System.Keys](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys)

Заменяющие последовательности - значения [SendKeys](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys.send#remarks)

---

# KeyboardMapper

Allows you to change the key codes or disable them.

When a key is pressed, checks if it is set in the *Mapper.cfg* file. 
If so, it intercepts the event. If a replacement sequence is specified, 
emulates its connector.

The program does not require administrator rights.
Minimum set of files: .exe and Mappings.cfg.

## Installation

1. Run the program. A tray icon will appear.
1. By pressing the necessary keys, get their names from the pop-up windows.
1. Stop the program from the context menu of the tray icon.
1. Fill in *Mappings.cfg* into the program folder.
1. Put the program at startup.

## *Mapper.cfg* format:

**Attention!** ALL keystrokes are intercepted, and their duties are 
performed in the pop-up window.

With these settings, the send letter key will behave like Esc, 
F1 won't work at all, and pressing F2 sequentially will press 
*1, 2, 3 and Enter*.

~~~
**Key_Name1**=**Overriding_Sequence1**
**Key_Name2**=**Overriding_Sequence2**

LaunchMail={Esc}
F1=
F2=123{Enter}
~~~

Key names - [System.Keys](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys) Values

Overriding sequences - [SendKeys](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys.send#remarks) Values
