import { useState } from "react";
import './styles.css';
import axios from 'axios';
import { useNavigate } from "react-router-dom"; // Importe o useNavigate

function Registrar() {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate(); // Inicialize o hook

    async function handleRegistrar(e) {
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
            const response = await axios.post('http://localhost:5177/api/Auth/register', data, {
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            console.log("Registro bem-sucedido:", response.data);
            alert("Usuário registrado com sucesso!");
            
            setUsername('');
            setEmail('');
            setPassword('');

            // Redireciona para a página de login
            navigate('/login');

        } catch (error) {
            console.error("Erro ao registrar:", error.response ? error.response.data : error.message);
            
            let errorMessage = "Falha no registro. Tente novamente.";
            
            if (error.response && error.response.data && error.response.data.errors) {
                const passwordErrors = error.response.data.errors.Password;
                if (passwordErrors && passwordErrors.length > 0) {
                    errorMessage = "Senha fraca: \n" + passwordErrors.join("\n");
                }
            }
            
            alert(errorMessage);
        }
    }

    return (
        <div className="login-wrapper">
            <div className="containerRegistrar">
                <h1>Cadastro de usuário</h1>
                <form onSubmit={handleRegistrar}>
                    <input
                        placeholder='username'
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        type="text"
                    /><br />

                    <input
                        placeholder='Email'
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        type="email"
                    /><br />

                    <input
                        placeholder='Password'
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        type="password"
                    /><br />

                    <button type="submit">Registrar</button>
                </form>
            </div>
        </div>
    )
}

export default Registrar;