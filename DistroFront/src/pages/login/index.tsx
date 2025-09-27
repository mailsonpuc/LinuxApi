import { useState, type FormEvent } from "react";
import './login.css';
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";
import { type AuthResponse } from "../../interfaces/IauthResponse";



export function Login() {
    // Definindo os estados para o nome de usuário e senha
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();

    // Tipagem correta para o evento de submit do formulário
    async function handleLogin(e: FormEvent<HTMLFormElement>) {
        e.preventDefault();

        // Adiciona a validação para campos vazios
        if (!userName || !password) {
            alert("Por favor, preencha todos os campos.");
            return;
        }

        const data = {
            userName,
            password,
        };

        try {
            // Requisição POST usando fetch
            const response = await fetch('https://apireact-dcarhnh2g3dtdehf.brazilsouth-01.azurewebsites.net/api/Auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            });

            // Verifica se a resposta não foi bem-sucedida (status 4xx ou 5xx)
            if (!response.ok) {
                // Tenta ler a mensagem de erro da API
                const errorData = await response.json();
                throw new Error(errorData.message || `Falha no login com status: ${response.status}`);
            }

            // Converte o corpo da resposta para JSON
            const authData: AuthResponse = await response.json();

            const decodedToken: any = jwtDecode(authData.token);
            const loggedInUserName = decodedToken.unique_name;

            // Armazena os dados
            localStorage.setItem('userName', loggedInUserName);
            localStorage.setItem('authToken', authData.token);

            localStorage.setItem('refreshToken', authData.refreshToken);
            localStorage.setItem('tokenExpiration', authData.expiration);


            alert("Login realizado com sucesso!");

            // Navega para a página inicial
            navigate("/", { replace: true });

            // Limpa os campos
            setUserName('');
            setPassword('');

        } catch (error: any) {
            console.error("Erro ao fazer login:", error.message);
            alert("Credenciais inválidas. Tente novamente.");
        }
    }

    return (
        <div className="login-wrapper">
            <div className="containerLogin">
                <h1>Entrar</h1>
                <form onSubmit={handleLogin}>
                    {/* INPUT: Username */}
                    <input
                        placeholder='Username'
                        value={userName}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setUserName(e.target.value)}
                        type="text"
                    /><br />

                    {/* INPUT: Password */}
                    <input
                        placeholder='Password'
                        value={password}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)}
                        type="password"
                    /><br />

                    <button type="submit">Entrar</button>
                </form>
            </div>
        </div>
    );
}

export default Login;