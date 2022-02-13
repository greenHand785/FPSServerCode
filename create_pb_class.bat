@echo off

:: 通讯协议
D:\MyProjects\FrameSyncFPSGame\develop\conf\protobuf-master\CodeGenerator\bin\Debug\net472\CodeGenerator.exe D:\MyProjects\FrameSyncFPSGame\develop\conf\proto\mod_proto\mod.proto --output D:\MyProjects\FrameSyncFPSGame\develop\server_code\Server\Server\ygy\game\pb\Mod.cs
D:\MyProjects\FrameSyncFPSGame\develop\conf\protobuf-master\CodeGenerator\bin\Debug\net472\CodeGenerator.exe D:\MyProjects\FrameSyncFPSGame\develop\conf\proto\mod_proto\login_mod.proto --output D:\MyProjects\FrameSyncFPSGame\develop\server_code\Server\Server\ygy\game\pb\LoginMod.cs
D:\MyProjects\FrameSyncFPSGame\develop\conf\protobuf-master\CodeGenerator\bin\Debug\net472\CodeGenerator.exe D:\MyProjects\FrameSyncFPSGame\develop\conf\proto\mod_proto\chat_mod.proto --output D:\MyProjects\FrameSyncFPSGame\develop\server_code\Server\Server\ygy\game\pb\ChatMod.cs

:: 存盘协议
D:\MyProjects\FrameSyncFPSGame\develop\conf\protobuf-master\CodeGenerator\bin\Debug\net472\CodeGenerator.exe D:\MyProjects\FrameSyncFPSGame\develop\conf\proto\mod_proto\character.proto --output D:\MyProjects\FrameSyncFPSGame\develop\server_code\Server\Server\ygy\game\db\CharacterMod.cs
pause