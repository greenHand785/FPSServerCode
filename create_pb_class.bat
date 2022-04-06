@echo off

:: 通讯协议
D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\protobuf-master\CodeGenerator\bin\Debug\net472\CodeGenerator.exe D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\proto\mod_proto\mod.proto --output D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\server_code\Server\Server\ygy\game\pb\Mod.cs
D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\protobuf-master\CodeGenerator\bin\Debug\net472\CodeGenerator.exe D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\proto\mod_proto\login_mod.proto --output D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\server_code\Server\Server\ygy\game\pb\LoginMod.cs
D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\protobuf-master\CodeGenerator\bin\Debug\net472\CodeGenerator.exe D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\proto\mod_proto\chat_mod.proto --output D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\server_code\Server\Server\ygy\game\pb\ChatMod.cs
D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\protobuf-master\CodeGenerator\bin\Debug\net472\CodeGenerator.exe D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\proto\mod_proto\hall_mod.proto --output D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\server_code\Server\Server\ygy\game\pb\HallMod.cs
D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\protobuf-master\CodeGenerator\bin\Debug\net472\CodeGenerator.exe D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\proto\mod_proto\enum_mod.proto --output D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\server_code\Server\Server\ygy\game\pb\EnumMod.cs


:: 存盘协议
D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\protobuf-master\CodeGenerator\bin\Debug\net472\CodeGenerator.exe D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\conf\proto\mod_proto\character.proto --output D:\YgyProject\ygy\ygyProject\bishe\FrameSyncFPSGame\develop\server_code\Server\Server\ygy\game\db\CharacterMod.cs
pause