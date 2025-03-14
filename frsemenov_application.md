# Семёнов Фёдор - "Игра на Unity с нуля"

### Группа: 10 - И - 3
### Электронная почта: fedor.semenov.2008@gmail.com
### Tg: @ToyvoF


**[ НАЗВАНИЕ ПРОЕКТА ]**

Находится в разработке

**[ ПРОБЛЕМНОЕ ПОЛЕ ]**

Souls-like - крайне популярный жанр игр, существующий уже почти 15 лет (с момента релиза Demon's souls), однако даже новейшие и самые высокобюджетные представители жанра (такие как Elden Ring) страдают от застоявшисся проблем жанра, а разработчики раз за разом допускают одни и те же ошибки при реализации механик игры. Самой серьёзной из них, на мой взгляд, является тривиальность избежания урона: абсолютное большинство игр используют систему "переката", делающего игрока неуязвимым на короткое время, или парирования (функционально то же самое - нужно нажимать одну и ту же кнопку в тайминг). Это обнуляет всё разнообразие врагов в игре, так как реакция игрока на любую угрозу одна - нажать кнопку уклонения. 
В своей игре я планирую переосмыслить эту систему, добавив до 15 уникальных способностей перемещения (вкупе с удобным интерфейсом для выбора и применения нужной в любой момент), каждая из которых даст игроку уникальный эффект, позволяющий избежать определённую атаку или наоборот, нанести урон в подходящей ситуации (см. подробнее в разделе Функциональные Требования). Я рассчитываю что, благодаря этому и другим нововведениям, моя игра станет действительно уникальной на фоне других представителей жанра и возможно даже вдохновит новое поколение souls-like игр.

**[ ЗАКАЗЧИК / ПОТЕНЦИАЛЬНАЯ АУДИТОРИЯ ]**

Если видение эстетической составляющей игры не изменится, целевой аудиторией станут почти все геймеры: в основном люди от 14 до 35 лет. 

**[ АППАРАТНЫЕ / ПРОГРАММНЫЕ ТРЕБОВАНИЯ ]**

О конкретных системных требованиях говорить рано, их невозможно предсказать на столь раннем этапе, однако изначальный выпуск игры планируется только на windows 10

Требования:
- наличие на устройстве операционной системы Windows 10 или выше

**[ ФУНКЦИОНАЛЬНЫЕ ТРЕБОВАНИЯ ]**
Артистическое видение игры, история мира и главного героя, сюжет и внешний вид врагов ещё не окончательно определены и будут дорабатываться по мере продвижения проекта, однако я уверен что смогу создать необходимые 3D-модели, эффекты и шейдеры с помощью Unity и Blender. 

Функциональные требования сформированы с точки зрения геймплея.

Программный продукт будет предоставлять следующие возможности:
* Настройки игры, в том числе гибкая настройка интерфейса выбора способностей.
* Сохранение прогресса игрока между запусками.
* Плавное премещение персонажа в 3D-пространстве, с учётом физики.
* Свободное исследование как минимум одной большой локации (верхней границей размера можно считать локацию Замогилье(англ. Limgrave) из игры elden ring), наполненной контентом на уровне завершенной игры.
* Возможность прохождения заскриптованного(scripted) фрагмента обучения, знакомящего игрока с основными механиками игры в интерактивном формате.
* Использование игроком (минимум) шести полностью уникальных способностей, у каждой из которых также будет 2 альтернативных варианта. (всего 18 вариантов)
* Использование игроком (минимум) трёх полностью уникальных видов оружия.
* Сражение игроком с (минимум) шестью полностью уникальными классами врагов, в каждом из которых в среднем будет 5 вариантов. (итого 30 уникальных врагов, не считая чисто визуальные различия)
* Сражение игроком с (минимум) тремя полноценными боссами.

**[ ПОХОЖИЕ / АНАЛОГИЧНЫЕ ПРОДУКТЫ ]**

Анализ игр того же жанра, таких как Elden Ring, Dark Souls 3 и (менее известная) Another Crab's Treasure показал, что:
* Elden ring хоть и имеет большой открытый мир, но сильно страдает от нехватки оригинального контента: большая часть врагов и боссов в игре повторяются по многу раз, оружия одного класса часто не имеют значимых геймплейных различий.
* Dark Souls 3 страдает от проблем всех предыдущих частей: дизайн локаций представлеются крайне неудобным для игрока, а большая часть боёв сводится к нажатию кнопки переката в нужный момент, что сильно снижает разнообразие геймплея на протяжении игры.
* Another Crab's Treasure страдает от неопределённости сеттинга, скудности бестиария врагов и, как и dark souls, тривиальности избежания урона. (хоть и в меньшей степени благодаря расширенной системе парирования)
Все эти проблемы я планирую исправить в своей игре

**[ ИНСТРУМЕНТЫ РАЗРАБОТКИ, ИНФОРМАЦИЯ О БД ]**

*	Unity и С# для основной части разработки
*	Json для сохранения данных
*	Blender для создания 3D-моделей
* Krita для создания текстур
* Для взаимодействия с БД в виде облачных сохранений планируется использовать возможности steam или SQLite

**[ ВОЗМОЖНЫЕ РИСКИ ]**

*   Нехватка времени на завершение побочных составляющих игры (к примеру графика, интерфейс, сценарий и сюжет), тестировку и дебаг
*   Неправильная оценка своих сил, как следствие - изменение размера проекта


