## Лабораторная работа № 2. Обнаружение и исправление ошибок

Целью лабораторной работы является изучение кодов обнаруживающих и исправляющих ошибки.

Языки разработки: С++ или С#. .

Общая постановка задачи: разработать консольную программу, выполняющую задание.

Срок выполнения работы: 2 недели.

В папке src приведены пример, состоящий из двух программ:

- loop - выделяет блок памяти, выводит на экран указатель на блок памяти, запускает бесконечный цикл, в котором выполняет цикл операций:
    - заполнение памяти случайными данными;
    - вычисление xor-суммы данных;
    - временная задержка;
    - пересчет xor-суммы данных;
    - проверка значения, если значение суммы изменяется - выводит сообщение об этом.
- main_process - ожидает от пользователя ввода указателя на блок памяти, запускает бесконечный цикл, в котором выполняет цикл операций:
    - выбор случайного байта;
    - изменение значения байта на инвертированное значение;
    - временная задерка и ожидание нажатия клавиши пользователя.

# Задание:

1. Изучить исходные коды и науиться "бить" память.
2. Исследовать xor-сумму:
    - изменить один, два... несколько бит в одном или нескольких битах;
    - сделать вывод об обнаруживаюзей способности.
3. Изучить и запрограммировать алгоритм CRC16, CRC32. Исследовать обнаруживающую способность.
4. Изучить коды, восстанавливающие ошибку: линейные коды, коды БЧХ, коды Рида-Соломона. Можно воспользоваться готовыми реализациями. Исследовать обнаруживающую способность.


# Читать:

[[0]](http://publ.lib.ru/ARCHIVES/B/BLEYHUT_Richard_E/_Bleyhut_R.E..html)
[[1]](https://ru.wikipedia.org/wiki/%D0%9A%D0%BE%D1%80%D1%80%D0%B5%D0%BA%D1%82%D0%B8%D1%80%D1%83%D1%8E%D1%89%D0%B8%D0%B9_%D0%BA%D0%BE%D0%B4)
[[2]](https://habr.com/ru/articles/328202/)
[[3]](https://habr.com/ru/companies/yadro/articles/336286/)

