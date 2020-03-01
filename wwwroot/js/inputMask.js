$(document).ready(function () {
    $(".telefone").inputmask("mask", { "mask": '(99)99999-9999' }, { reverse: true });//14
    $(".cpf").inputmask("mask", { "mask": '999.999.999-99' }, { reverse: true });//14
    $(".cnpj").inputmask("mask", { "mask": '99.999.999/9999-99' }, { reverse: true });//18
    $(".cep").inputmask("mask", { "mask": '99999-999' });
    $(".nascimento").inputmask("mask", { "mask": '99/99/9999' });
    $(".preco").inputmask("mask", { "mask": '999.999,99' }, { reverse: true });
    $(".valor").inputmask("mask", { "mask": '#.##9,99' }, { reverse: true });
    $(".ip").inputmask("mask", { "mask": '999.999.999.999' });
    $(".preco").inputmask("mask", { "mask": '999.999,99' }, { reverse: true });
    $(".porcento").inputmask("mask", { "mask": '9,999' }, { reverse: true });
    $(".valor").inputmask("mask", { "mask": '#.##9,99' }, { reverse: true });
});