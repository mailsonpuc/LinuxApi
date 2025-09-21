import { useState } from "react";
import './styles.css'; 
import axios from 'axios';
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

function Login() {
    const [userName, setUserName] = useState("mailson");
    const [password, setPassword] = useState("Nunsey#Senha321");
    const navigate = useNavigate();

    async function handleLogin(e) {
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
            const response = await axios.post('http://localhost:5177/api/Auth/login', data, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            const decodedToken = jwtDecode(response.data.token);
            const loggedInUserName = decodedToken.unique_name;
            
            localStorage.setItem('userName', loggedInUserName);
            localStorage.setItem('authToken', response.data.token);

            alert("Login realizado com sucesso! 🎉");
            
            navigate("/", { replace: true });
            
            setUserName('');
            setPassword('');

        } catch (error) {
            console.error("Erro ao fazer login:", error.response ? error.response.data : error.message);
            alert("Credenciais inválidas. Tente novamente.");
        }
    }

    return (
        <div className="login-wrapper">
            <div className="containerLogin">
                <h1>Entrar</h1>
                <form onSubmit={handleLogin}>
                    <input
                        placeholder='Username'
                        value={userName}
                        onChange={(e) => setUserName(e.target.value)}
                        type="text"
                    /><br />

                    <input
                        placeholder='Password'
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        type="password"
                    /><br />

                    <button type="submit">Entrar</button>
                </form>
            </div>
        </div>
    );
}

export default Login;