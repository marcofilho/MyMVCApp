function SetModal() {

    $(document).ready(function () {
        $(function () {
            $.ajaxSetup({ cache: false });

            $("a[data-modal]").on("click",
                function (e) {
                    $('#myModalContent').load(this.href,
                        function () {
                            $('#myModal').modal({
                                keyboard: true
                            },
                                'show');
                            bindForm(this);
                        });
                    return false;
                });
        });
    });
}

function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    $('#AddressTarget').load(result.url);                         
                } else {
                    $('#myModalContent').html(result);
                    bindForm(dialog);
                }
            }
        });

        SetModal();
        return false;
    });
}

function BuscaCep() {
    $(document).ready(function () {

        function limpa_formulário_cep() {
            $("#Address_Street").val("");
            $("#Address_Neighborhood").val("");
            $("#Address_City").val("");
            $("#Address_State").val("");
        }

        $("#Endereco_Cep").blur(function () {

            var cep = $(this).val().replace(/\D/g, '');

            if (cep != "") {

                var validacep = /^[0-9]{8}$/;

                if (validacep.test(cep)) {

                    $("#Address_Street").val("...");
                    $("#Address_Neighborhood").val("...");
                    $("#Address_City").val("...");
                    $("#Address_State").val("...");

                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                $("#Address_Street").val(dados.logradouro);
                                $("#Address_Neighborhood").val(dados.bairro);
                                $("#Endereco_Cidade").val(dados.localidade);
                                $("#Address_State").val(dados.uf);
                            }    
                            else {
                                limpa_formulário_cep();
                                alert("Postal Code not found.");
                            }
                        });
                }    
                else {
                    limpa_formulário_cep();
                    alert("Postal code format invalid.");
                }
            }    
            else {
                limpa_formulário_cep();
            }
        });
    });
}

$(document).ready(function () {
    $("#msg_box").fadeOut(2500);
});