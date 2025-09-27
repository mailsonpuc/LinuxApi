import { useState, type FormEvent } from "react";
import './registrar.css';
import { useNavigate } from "react-router-dom"; 
import { type RegisterResponse } from "../../interfaces/IregisterResponse";


export function Registrar() {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    // Tipagem correta para o evento de submit do formulário
    async function handleRegistrar(e: FormEvent<HTMLFormElement>) {
        e.preventDefault();

        if (!username || !email || !password) {
            alert("Por favor, preencha todos os campos.");
            return;
        }

        const data = {
            username,
            email,
            password,
        };

        try {
            // Requisição POST usando fetch
            const response = await fetch('http://localhost:5177/api/Auth/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

            // Converte o corpo da resposta para JSON
            const responseData: RegisterResponse = await response.json();

            // Verifica se a resposta não foi bem-sucedida (status 4xx ou 5xx)
            if (!response.ok) {
                // Se a API não retornar uma mensagem no erro, usaremos a mensagem padrão
                throw new Error(responseData.message || `Falha no registro com status: ${response.status}.`);
            }
            
            // Verifica o status do corpo da resposta, apenas para redundância, se o status for 200/201
            if (responseData.status !== "Success") {
                throw new Error(responseData.message || "Registro falhou após a requisição.");
            }

            console.log("Registro bem-sucedido:", responseData);
            alert("Usuário registrado com sucesso! 🎉");
            
            // Limpa os campos
            setUsername('');
            setEmail('');
            setPassword('');

            // Redireciona para a página de login
            navigate('/login');

        } catch (error: any) {
            // Captura e exibe a mensagem de erro da API ou a mensagem de falha da rede/código
            console.error("Erro ao registrar:", error.message);
            alert(error.message);
        }
    }

    return (
        <div className="login-wrapper">
            <div className="containerRegistrar">
                <h1>Cadastro de usuário</h1>
                <form onSubmit={handleRegistrar}>
                    {/* INPUT: Username */}
                    <input
                        placeholder='Username'
                        value={username}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setUsername(e.target.value)}
                        type="text"
                    /><br />

                    {/* INPUT: Email */}
                    <input
                        placeholder='Email'
                        value={email}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setEmail(e.target.value)}
                        type="email"
                    /><br />

                    {/* INPUT: Password */}
                    <input
                        placeholder='Password'
                        value={password}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)}
                        type="password"
                    /><br />

                    <button type="submit">Registrar</button>
                </form>
            </div>
        </div>
    )
}

export default Registrar;