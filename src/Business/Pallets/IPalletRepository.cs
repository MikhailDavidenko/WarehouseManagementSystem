using WarehouseManagementSystem.Domain;

namespace WarehouseManagementSystem.Business.Pallets;

public interface IPalletRepository
{
    /// <summary>
    /// Получает паллету по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор паллеты.</param>
    /// <returns>Паллета с указанным идентификатором, или null, если не найдена</returns>
    Task<Pallet?> GetPalletByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает список всех паллет
    /// </summary>
    /// <returns>Список всех паллет</returns>
    Task<IReadOnlyList<Pallet>> GetPalletsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает список паллет с пагинацией.
    /// </summary>
    /// <param name="offset">Начальный индекс</param>
    /// <param name="limit">Количество записей</param>
    /// <returns>Список паллет с пагинацией</returns>
    Task<IReadOnlyList<Pallet>> GetPalletsWithPaginationAsync(int offset = 0, int limit = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает список паллет, отсортированных по дате окончания срока годности и весу
    /// </summary>
    /// <returns>Список паллет, отсортированных по дате окончания срока годности и весу</returns>
    Task<IReadOnlyList<Pallet>> GetPalletsSortedByExpirationAndWeightAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает список трех паллет с самой ранней датой окончания срока годности, отсортированных по весу
    /// </summary>
    /// <returns>Список трех паллет с самой ранней датой окончания срока годности, отсортированных по весу</returns>
    Task<IReadOnlyList<Pallet>> GetTopThreeWithLatestExpirationDateSortedByWeightAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавляет новую паллету
    /// </summary>
    /// <param name="pallet">Новая паллета</param>
    /// <returns>Идентификатор добавленной паллеты</returns>
    Task AddPalletAsync(Pallet pallet, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновляет существующую паллету
    /// </summary>
    /// <param name="pallet">Паллета для обновления</param>
    Task UpdatePalletAsync(Pallet pallet, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет паллету по идентификатору
    /// </summary>
    /// <param name="palletId">Идентификатор паллеты для удаления</param>
    Task DeleteByIdAsync(Guid palletId, CancellationToken cancellationToken = default);
}
