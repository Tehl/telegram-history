# Telegram History
Exports Telegram conversation history to JSON and flat text files.

Written in C# using [TLSharp](https://github.com/sochix/TLSharp)

## Getting started
In order to use the export tool, you will need a Telegram Developer account. Create a developer account at https://my.telegram.org/ and then access the [API development tools](https://my.telegram.org/apps) to retrieve your **api_id** and **api_hash**.

## For users
If you just want to export a conversation, download the `dist` folder and run TelegramHistory.exe directly. You will need to add your **api_id** and **api_hash** to `TelegramHistory.exe.config` using Notepad or a similar text editor before you begin. You can also add your phone number in [international format](https://telegram.org/faq#login-and-sms) to avoid having to input it each time you run the app.

## For developers
TelegramHistory uses a forked version of TLSharp available [here](https://github.com/Tehl/TLSharp/). I've submitted a pull request incoporating my changes, so I'll remove/update this message appropriately if it gets merged. The repository includes a release build of the library anyway, so this is only relevent if you need to debug or change the underlying library.

## License
TelegramHistory is licensed under the [MIT License](https://github.com/Tehl/telegram-history/blob/master/LICENSE).
