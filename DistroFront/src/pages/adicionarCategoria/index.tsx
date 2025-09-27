import { useState, type FormEvent } from "react";
import './addCategoria.css';


export function AdicionarCategoria() {
    const [nome, setNome] = useState('');

    // A tipagem correta para o evento de submit de um formulário.
    function handleRegister(e: FormEvent<HTMLFormElement>) {
        e.preventDefault();

        // O objeto a ser enviado. Como você usa GUIDs, não há campo ID aqui,
        // pois ele será gerado pelo backend.
        const novaCategoria = {
            nome: nome,
        };

        fetch("http://localhost:5177/api/Categorias", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(novaCategoria)
        })
            .then(response => {
                if (!response.ok) {
                    // Lançamos um erro se a resposta não for 2xx
                    throw new Error(`Falha na API com status: ${response.status}`);
                }
                return response.json();
            })
            .then(() => {
                alert("Categoria adicionada com sucesso!");
                // Limpa o campo
                setNome('');
            })
            .catch(error => {
                console.error("Erro ao enviar dados:", error);
                alert(`Falha ao adicionar categoria: ${error.message}`);
            });
    }

    return (
        <div className="login-wrapper">
            <div className="containerLogin">

                <form onSubmit={handleRegister}>
                    <label>Adicionar uma nova Categoria</label><br />

                    {/* INPUT: Nome */}
                    <input
                        placeholder="Nome"
                        value={nome}
                        // Tipagem correta para o evento de mudança (change event) de um input.
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setNome(e.target.value)}
                        required
                        maxLength={100}
                    /><br /><br />

                    <button type="submit">Adicionar</button>
                </form>
            </div>
        </div>
    );
}