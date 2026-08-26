// ======================================================
// 1. ESTADO DA APLICAÇÃO
// ======================================================

let assentoSelecionado = null;
let clienteSelecionado = null;
let sessaoSelecionada = null;
let formaPagamentoSelecionada = null;
let compraEmProcessamento = false;


// ======================================================
// 2. CARREGAR FILMES
// ======================================================

carregarFilmes();


function carregarFilmes() {

    fetch('/api/Filmes')
        .then(response => {

            if (!response.ok) {
                throw new Error(
                    'Erro ao buscar filmes.'
                );
            }

            return response.json();
        })
        .then(filmes => {

            const listaFilmes =
                document.getElementById(
                    'lista-filmes'
                );

            listaFilmes.innerHTML = '';

            filmes.forEach(filme => {

                const card =
                    document.createElement('div');

                card.classList.add(
                    'filme-card'
                );

                card.innerHTML = `
                    <div class="filme-poster-container">

                        <img
                            class="filme-poster"
                            src="${filme.posterUrl}"
                            alt="Pôster de ${filme.titulo}"
                        >

                    </div>

                    <div class="filme-info">

                        <h3>
                            ${filme.titulo}
                        </h3>

                        <p>
                            <strong>Gênero:</strong>
                            ${filme.genero}
                        </p>

                        <p>
                            <strong>Duração:</strong>
                            ${filme.duracaoMinutos} min
                        </p>

                        <p>
                            <strong>Classificação:</strong>
                            ${filme.classificacaoIndicativa}
                        </p>

                        <button onclick="verSessoes(${filme.id})">
                            Ver sessões
                        </button>

                    </div>
                `;

                listaFilmes.appendChild(
                    card
                );
            });
        })
        .catch(error => {

            console.error(
                'Erro ao buscar filmes:',
                error
            );
        });
}


// ======================================================
// 3. SESSÕES
// ======================================================

function verSessoes(filmeId) {

    fetch(`/api/Filmes/${filmeId}`)
        .then(response => {

            if (!response.ok) {
                throw new Error(
                    'Erro ao buscar filme.'
                );
            }

            return response.json();
        })
        .then(filme => {

            document.getElementById(
                'sessao-filme-titulo'
            ).textContent =
                filme.titulo;

            return fetch('/api/Sessoes');
        })
        .then(response => {

            if (!response.ok) {
                throw new Error(
                    'Erro ao buscar sessões.'
                );
            }

            return response.json();
        })
        .then(sessoes => {

            const sessoesDoFilme =
                sessoes.filter(
                    sessao =>
                        sessao.filmeId === filmeId
                );

            const listaFilmes =
                document.getElementById(
                    'lista-filmes'
                );

            const tituloFilmes =
                document.getElementById(
                    'titulo-filmes'
                );

            const areaSessoes =
                document.getElementById(
                    'area-sessoes'
                );

            const listaSessoes =
                document.getElementById(
                    'lista-sessoes'
                );

            listaFilmes.style.display =
                'none';

            tituloFilmes.style.display =
                'none';

            areaSessoes.style.display =
                'block';

            listaSessoes.innerHTML = '';

            if (sessoesDoFilme.length === 0) {

                listaSessoes.innerHTML = `
                    <p>
                        Nenhuma sessão disponível para este filme.
                    </p>
                `;

                return;
            }

            sessoesDoFilme.forEach(
                sessao => {

                    criarCardSessao(
                        sessao,
                        listaSessoes
                    );
                }
            );
        })
        .catch(error => {

            console.error(
                'Erro ao buscar sessões:',
                error
            );
        });
}


function criarCardSessao(
    sessao,
    listaSessoes
) {

    const cardSessao =
        document.createElement(
            'div'
        );

    cardSessao.classList.add(
        'sessao-card'
    );

    const data =
        new Date(
            sessao.dataHora
        );

    const dataFormatada =
        data.toLocaleDateString(
            'pt-BR'
        );

    const horarioFormatado =
        data.toLocaleTimeString(
            'pt-BR',
            {
                hour: '2-digit',
                minute: '2-digit'
            }
        );

    const precoFormatado =
        Number(
            sessao.precoIngresso
        )
            .toFixed(2)
            .replace('.', ',');

    cardSessao.innerHTML = `
        <h3>
            ${dataFormatada}
        </h3>

        <p>
            <strong>Horário:</strong>
            ${horarioFormatado}
        </p>

        <p>
            <strong>Sala:</strong>
            ${sessao.salaId}
        </p>

        <p>
            <strong>Preço:</strong>
            R$ ${precoFormatado}
        </p>

        <button
            onclick="escolherSessao(
                ${sessao.id},
                ${sessao.precoIngresso}
            )">

            Escolher sessão

        </button>
    `;

    listaSessoes.appendChild(
        cardSessao
    );
}


function voltarParaFilmes() {

    document.getElementById(
        'lista-filmes'
    ).style.display =
        'grid';

    document.getElementById(
        'titulo-filmes'
    ).style.display =
        'block';

    document.getElementById(
        'area-sessoes'
    ).style.display =
        'none';
}


// ======================================================
// 4. ASSENTOS
// ======================================================

function escolherSessao(
    sessaoId,
    precoIngresso
) {

    sessaoSelecionada = {
        id: sessaoId,
        precoIngresso: precoIngresso
    };

    assentoSelecionado =
        null;

    const btnContinuar =
        document.getElementById(
            'btn-continuar-assento'
        );

    btnContinuar.disabled =
        true;

    fetch(
        `/api/Assentos/sessao/${sessaoId}`
    )
        .then(response => {

            if (!response.ok) {
                throw new Error(
                    'Erro ao buscar assentos.'
                );
            }

            return response.json();
        })
        .then(assentos => {

            document.getElementById(
                'area-sessoes'
            ).style.display =
                'none';

            document.getElementById(
                'area-assentos'
            ).style.display =
                'block';

            const listaAssentos =
                document.getElementById(
                    'lista-assentos'
                );

            listaAssentos.innerHTML =
                '';

            ordenarAssentos(
                assentos
            );

            assentos.forEach(
                assento => {

                    criarBotaoAssento(
                        assento,
                        listaAssentos
                    );
                }
            );
        })
        .catch(error => {

            console.error(
                'Erro ao buscar assentos:',
                error
            );
        });
}


function ordenarAssentos(
    assentos
) {

    assentos.sort(
        (a, b) => {

            const letraA =
                a.codigo.charAt(0);

            const letraB =
                b.codigo.charAt(0);

            if (
                letraA !== letraB
            ) {

                return letraA.localeCompare(
                    letraB
                );
            }

            const numeroA =
                parseInt(
                    a.codigo.substring(1)
                );

            const numeroB =
                parseInt(
                    b.codigo.substring(1)
                );

            return numeroA - numeroB;
        }
    );
}


function criarBotaoAssento(
    assento,
    listaAssentos
) {

    const botaoAssento =
        document.createElement(
            'button'
        );

    botaoAssento.classList.add(
        'assento'
    );

    botaoAssento.textContent =
        assento.codigo;

    if (assento.ocupado) {

        botaoAssento.classList.add(
            'ocupado'
        );

        botaoAssento.disabled =
            true;

    } else {

        botaoAssento.classList.add(
            'livre'
        );

        botaoAssento.onclick =
            function () {

                selecionarAssento(
                    botaoAssento,
                    assento
                );
            };
    }

    listaAssentos.appendChild(
        botaoAssento
    );
}


function selecionarAssento(
    botao,
    assento
) {

    document
        .querySelectorAll(
            '.assento.selecionado'
        )
        .forEach(
            item => {

                item.classList.remove(
                    'selecionado'
                );
            }
        );

    botao.classList.add(
        'selecionado'
    );

    assentoSelecionado =
        assento;

    document.getElementById(
        'btn-continuar-assento'
    ).disabled =
        false;
}


function voltarParaSessoes() {

    document.getElementById(
        'area-assentos'
    ).style.display =
        'none';

    document.getElementById(
        'area-sessoes'
    ).style.display =
        'block';
}


function continuarComAssento() {

    if (!assentoSelecionado) {
        return;
    }

    document.getElementById(
        'area-assentos'
    ).style.display =
        'none';

    document.getElementById(
        'area-cliente'
    ).style.display =
        'block';

    limparErrosCampos();
}


function voltarParaAssentos() {

    document.getElementById(
        'area-cliente'
    ).style.display =
        'none';

    document.getElementById(
        'area-assentos'
    ).style.display =
        'block';

    limparErrosCampos();
}


// ======================================================
// 5. VALIDAÇÃO DO FORMULÁRIO
// ======================================================

function mostrarErroCampo(
    idCampo,
    idErro,
    mensagem
) {

    const campo =
        document.getElementById(
            idCampo
        );

    const erro =
        document.getElementById(
            idErro
        );

    campo.classList.add(
        'campo-invalido'
    );

    erro.textContent =
        mensagem;

    erro.classList.add(
        'visivel'
    );
}


function limparErroCampo(
    idCampo,
    idErro
) {

    const campo =
        document.getElementById(
            idCampo
        );

    const erro =
        document.getElementById(
            idErro
        );

    campo.classList.remove(
        'campo-invalido'
    );

    erro.textContent =
        '';

    erro.classList.remove(
        'visivel'
    );
}


function limparErrosCampos() {

    document
        .querySelectorAll(
            '.campo-invalido'
        )
        .forEach(
            campo => {

                campo.classList.remove(
                    'campo-invalido'
                );
            }
        );

    document
        .querySelectorAll(
            '.erro-campo'
        )
        .forEach(
            erro => {

                erro.textContent =
                    '';

                erro.classList.remove(
                    'visivel'
                );
            }
        );

    limparErroCliente();
}


// ======================================================
// 6. ERRO GERAL DA API
// ======================================================

function mostrarErroCliente(
    mensagem
) {

    const elemento =
        document.getElementById(
            'mensagem-cliente'
        );

    elemento.innerHTML = `
        <span class="icone-erro">
            !
        </span>

        <span>
            ${mensagem}
        </span>
    `;

    elemento.className =
        'mensagem-formulario erro';

    elemento.style.display =
        'flex';
}


function limparErroCliente() {

    const elemento =
        document.getElementById(
            'mensagem-cliente'
        );

    if (!elemento) {
        return;
    }

    elemento.innerHTML =
        '';

    elemento.style.display =
        'none';

    elemento.className =
        'mensagem-formulario';
}


// ======================================================
// 7. PROCESSAMENTO DA COMPRA
// ======================================================

function iniciarProcessamentoCompra() {

    compraEmProcessamento =
        true;

    const botao =
        document.querySelector(
            '.cliente-acoes button:last-child'
        );

    if (!botao) {
        return;
    }

    botao.disabled =
        true;

    botao.textContent =
        'Processando...';
}


function finalizarProcessamentoCompra() {

    compraEmProcessamento =
        false;

    const botao =
        document.querySelector(
            '.cliente-acoes button:last-child'
        );

    if (!botao) {
        return;
    }

    botao.disabled =
        false;

    botao.textContent =
        'Continuar';
}


// ======================================================
// 8. CADASTRAR CLIENTE
// ======================================================

function continuarCliente() {

    if (compraEmProcessamento) {
        return;
    }

    limparErrosCampos();

    const nome =
        document.getElementById(
            'cliente-nome'
        ).value.trim();

    const email =
        document.getElementById(
            'cliente-email'
        ).value.trim();

    const telefone =
        document.getElementById(
            'cliente-telefone'
        ).value.trim();

    const nascimento =
        document.getElementById(
            'cliente-nascimento'
        ).value;

    const formaPagamento =
        document.getElementById(
            'forma-pagamento'
        ).value;

    let temErro =
        false;


    // ==============================
    // NOME
    // ==============================

    const nomeRegex =
        /^[A-Za-zÀ-ÖØ-öø-ÿ' -]+$/;

    if (!nome) {

        mostrarErroCampo(
            'cliente-nome',
            'erro-nome',
            'Informe seu nome.'
        );

        temErro =
            true;

    } else if (
        nome.length < 3 ||
        !nomeRegex.test(nome)
    ) {

        mostrarErroCampo(
            'cliente-nome',
            'erro-nome',
            'Use apenas letras e informe um nome válido.'
        );

        temErro =
            true;
    }


    // ==============================
    // E-MAIL
    // ==============================

    const emailRegex =
        /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$/;

    if (!email) {

        mostrarErroCampo(
            'cliente-email',
            'erro-email',
            'Informe seu e-mail.'
        );

        temErro =
            true;

    } else if (
        !emailRegex.test(email)
    ) {

        mostrarErroCampo(
            'cliente-email',
            'erro-email',
            'Digite um e-mail válido. Exemplo: nome@email.com'
        );

        temErro =
            true;
    }


    // ==============================
    // TELEFONE
    // ==============================

    const telefoneRegex =
        /^[0-9]{10,11}$/;

    if (!telefone) {

        mostrarErroCampo(
            'cliente-telefone',
            'erro-telefone',
            'Informe seu telefone.'
        );

        temErro =
            true;

    } else if (
        !telefoneRegex.test(
            telefone
        )
    ) {

        mostrarErroCampo(
            'cliente-telefone',
            'erro-telefone',
            'Digite somente números, incluindo o DDD.'
        );

        temErro =
            true;
    }


    // ==============================
    // DATA DE NASCIMENTO
    // ==============================

    if (!nascimento) {

        mostrarErroCampo(
            'cliente-nascimento',
            'erro-nascimento',
            'Informe sua data de nascimento.'
        );

        temErro =
            true;

    } else {

        const dataNascimento =
            new Date(
                nascimento +
                'T00:00:00'
            );

        const hoje =
            new Date();

        hoje.setHours(
            0,
            0,
            0,
            0
        );

        if (
            dataNascimento > hoje
        ) {

            mostrarErroCampo(
                'cliente-nascimento',
                'erro-nascimento',
                'A data de nascimento não pode ser futura.'
            );

            temErro =
                true;
        }
    }


    // ==============================
    // FORMA DE PAGAMENTO
    // ==============================

    if (!formaPagamento) {

        mostrarErroCampo(
            'forma-pagamento',
            'erro-pagamento',
            'Selecione uma forma de pagamento.'
        );

        temErro =
            true;
    }


    if (temErro) {
        return;
    }


    // ==============================
    // SESSÃO / ASSENTO
    // ==============================

    if (
        !assentoSelecionado ||
        !sessaoSelecionada
    ) {

        mostrarErroCliente(
            'Sessão ou assento não selecionado.'
        );

        return;
    }


    formaPagamentoSelecionada =
        formaPagamento;


    const cliente = {

        nome:
            nome,

        email:
            email,

        telefone:
            telefone,

        dataNascimento:
            nascimento
    };


    // A PARTIR DAQUI BLOQUEIA DUPLO CLIQUE
    iniciarProcessamentoCompra();


    cadastrarCliente(
        cliente
    );
}


// ======================================================
// 9. REQUISIÇÃO - CLIENTE
// ======================================================

function cadastrarCliente(
    cliente
) {

    fetch('/api/Clientes', {

        method:
            'POST',

        headers: {
            'Content-Type':
                'application/json'
        },

        body:
            JSON.stringify(
                cliente
            )
    })
        .then(response => {

            if (!response.ok) {

                return response
                    .text()
                    .then(
                        mensagem => {

                            throw new Error(
                                mensagem ||
                                'Erro ao cadastrar cliente.'
                            );
                        }
                    );
            }

            return response.json();
        })
        .then(clienteCriado => {

            clienteSelecionado =
                clienteCriado;

            limparErrosCampos();

            criarIngresso();
        })
        .catch(error => {

            finalizarProcessamentoCompra();

            console.error(
                'Erro ao cadastrar cliente:',
                error
            );

            mostrarErroCliente(
                error.message ||
                'Não foi possível cadastrar o cliente.'
            );
        });
}


// ======================================================
// 10. CRIAR INGRESSO
// ======================================================

function criarIngresso() {

    const ingresso = {

        clienteId:
            clienteSelecionado.id,

        sessaoId:
            sessaoSelecionada.id,

        assento:
            assentoSelecionado.codigo,

        precoPago:
            sessaoSelecionada.precoIngresso
    };


    fetch('/api/Ingressos', {

        method:
            'POST',

        headers: {
            'Content-Type':
                'application/json'
        },

        body:
            JSON.stringify(
                ingresso
            )
    })
        .then(response => {

            if (!response.ok) {

                return response
                    .text()
                    .then(
                        mensagem => {

                            throw new Error(
                                mensagem ||
                                'Erro ao criar ingresso.'
                            );
                        }
                    );
            }

            return response.json();
        })
        .then(() => {

            criarCompra();
        })
        .catch(error => {

            finalizarProcessamentoCompra();

            console.error(
                'Erro ao criar ingresso:',
                error
            );

            mostrarErroCliente(
                error.message ||
                'Não foi possível criar o ingresso.'
            );

            document.getElementById(
                'area-cliente'
            ).style.display =
                'block';
        });
}


// ======================================================
// 11. CRIAR COMPRA
// ======================================================

function criarCompra() {

    const compra = {

        clienteId:
            clienteSelecionado.id,

        valorTotal:
            sessaoSelecionada.precoIngresso,

        formaPagamento:
            formaPagamentoSelecionada
    };


    fetch('/api/Compras', {

        method:
            'POST',

        headers: {
            'Content-Type':
                'application/json'
        },

        body:
            JSON.stringify(
                compra
            )
    })
        .then(response => {

            if (!response.ok) {
                throw new Error(
                    'Erro ao criar compra.'
                );
            }

            return response.json();
        })
        .then(compraCriada => {

            mostrarCompraFinalizada(
                compraCriada
            );
        })
        .catch(error => {

            finalizarProcessamentoCompra();

            console.error(
                'Erro ao finalizar compra:',
                error
            );

            mostrarErroCliente(
                'Não foi possível finalizar a compra.'
            );

            document.getElementById(
                'area-cliente'
            ).style.display =
                'block';
        });
}


// ======================================================
// 12. COMPRA FINALIZADA
// ======================================================

function mostrarCompraFinalizada(
    compra
) {

    compraEmProcessamento =
        false;

    document.getElementById(
        'area-cliente'
    ).style.display =
        'none';

    document.getElementById(
        'titulo-filmes'
    ).style.display =
        'none';

    document.getElementById(
        'lista-filmes'
    ).style.display =
        'none';

    document.getElementById(
        'area-sucesso'
    ).style.display =
        'block';

    document.getElementById(
        'resumo-cliente'
    ).textContent =
        clienteSelecionado.nome;

    document.getElementById(
        'resumo-assento'
    ).textContent =
        assentoSelecionado.codigo;

    document.getElementById(
        'resumo-valor'
    ).textContent =
        formatarDinheiro(
            sessaoSelecionada
                .precoIngresso
        );

    document.getElementById(
        'resumo-pagamento'
    ).textContent =
        compra.formaPagamento;
}


// ======================================================
// 13. FORMATADORES
// ======================================================

function formatarDinheiro(
    valor
) {

    return `R$ ${Number(valor)
        .toFixed(2)
        .replace('.', ',')}`;
}


// ======================================================
// 14. NOVA COMPRA
// ======================================================

function novaCompra() {

    assentoSelecionado =
        null;

    clienteSelecionado =
        null;

    sessaoSelecionada =
        null;

    formaPagamentoSelecionada =
        null;


    finalizarProcessamentoCompra();


    document.getElementById(
        'area-sucesso'
    ).style.display =
        'none';

    document.getElementById(
        'titulo-filmes'
    ).style.display =
        'block';

    document.getElementById(
        'lista-filmes'
    ).style.display =
        'grid';

    document.getElementById(
        'area-sessoes'
    ).style.display =
        'none';

    document.getElementById(
        'area-assentos'
    ).style.display =
        'none';

    document.getElementById(
        'area-cliente'
    ).style.display =
        'none';


    limparFormularioCliente();

    limparErrosCampos();


    const btnContinuar =
        document.getElementById(
            'btn-continuar-assento'
        );

    btnContinuar.disabled =
        true;


    document
        .querySelectorAll(
            '.assento.selecionado'
        )
        .forEach(
            item => {

                item.classList.remove(
                    'selecionado'
                );
            }
        );
}


function limparFormularioCliente() {

    document.getElementById(
        'cliente-nome'
    ).value =
        '';

    document.getElementById(
        'cliente-email'
    ).value =
        '';

    document.getElementById(
        'cliente-telefone'
    ).value =
        '';

    document.getElementById(
        'cliente-nascimento'
    ).value =
        '';

    document.getElementById(
        'forma-pagamento'
    ).value =
        '';
}


// ======================================================
// 15. LIMPAR ERROS DURANTE A DIGITAÇÃO
// ======================================================

document
    .getElementById(
        'cliente-nome'
    )
    .addEventListener(
        'input',
        function () {

            limparErroCampo(
                'cliente-nome',
                'erro-nome'
            );
        }
    );


document
    .getElementById(
        'cliente-email'
    )
    .addEventListener(
        'input',
        function () {

            limparErroCampo(
                'cliente-email',
                'erro-email'
            );
        }
    );


document
    .getElementById(
        'cliente-telefone'
    )
    .addEventListener(
        'input',
        function () {

            limparErroCampo(
                'cliente-telefone',
                'erro-telefone'
            );
        }
    );


document
    .getElementById(
        'cliente-nascimento'
    )
    .addEventListener(
        'change',
        function () {

            limparErroCampo(
                'cliente-nascimento',
                'erro-nascimento'
            );
        }
    );


document
    .getElementById(
        'forma-pagamento'
    )
    .addEventListener(
        'change',
        function () {

            limparErroCampo(
                'forma-pagamento',
                'erro-pagamento'
            );
        }
    );