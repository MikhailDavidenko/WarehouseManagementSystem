using WarehouseManagementSystem.Services;

var inputService = new ConsoleInputService();
var displayService = new ConsolePalletDisplayService();

var pallets = inputService.GetPallets();

displayService.DisplayPalletsGroupedByExpiration(pallets);
displayService.DisplayTopThreePalletsWithLongestShelfLife(pallets);

