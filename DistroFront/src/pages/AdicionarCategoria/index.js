import { useState, useEffect } from "react";
import './styles.css';


function AdicionarCategoria() {
    const [nome, setNome] = useState('');


    //funçao registrar categoria na api
    function handleRegister(e) {
        e.preventDefault();

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
                    throw new Error("Erro ao adicionar categoria.");
                }
                return response.json();
            })
            .then(data => {
                alert("Categoria adicionada com sucesso!");
                // Limpa os campos

                setNome('');

            })
            .catch(error => {
                console.error("Erro ao enviar dados:", error);
                alert("Falha ao adicionar categoria.");
            });
    }


    return (
        <div className="login-wrapper">
            <div className="containerLogin">

                <form onSubmit={handleRegister}>
                    <label>Adicionar uma nova Categoria</label><br />


                    <input
                        placeholder="Nome"
                        value={nome}
                        onChange={(e) => setNome(e.target.value)}
                        required
                        maxLength={100}
                    /><br /><br />


                    <button type="submit">Adicionar</button>
                </form>
            </div>
        </div>
    );

}


export default AdicionarCategoria;