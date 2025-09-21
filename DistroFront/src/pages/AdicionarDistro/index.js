import { useState, useEffect } from "react";
import './styles.css';
import Loading from '../../components/Loading';

function AdicionarDistro() {
    const [imageUrl, setimageUrl] = useState('');
    const [nome, setNome] = useState('');
    const [descricao, setDescricao] = useState('');
    const [iso, setIso] = useState('');
    const [categoriaid, setCategoriaid] = useState('');

    const [categorias, setCategorias] = useState([]);
    const [loadingCategorias, setLoadingCategorias] = useState(true); // Novo estado de loading

    useEffect(() => {
        setLoadingCategorias(true); // Inicia o loading
        
        fetch("http://localhost:5177/api/Categorias")
            .then(response => response.json())
            .then(data => {
                setCategorias(data);
                setLoadingCategorias(false); // Fim do loading
            })
            .catch(error => {
                console.error("Erro ao buscar categorias:", error);
                setLoadingCategorias(false); // Mesmo com erro, tira o loading
            });
    }, []);

    function handleRegister(e) {
        e.preventDefault();

        const novaDistro = {
            imageUrl: imageUrl,
            nome: nome,
            descricao: descricao,
            iso: iso,
            categoriaId: categoriaid
        };

        fetch("http://localhost:5177/api/Distros", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(novaDistro)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error("Erro ao adicionar distro.");
                }
                return response.json();
            })
            .then(data => {
                alert("Distro adicionada com sucesso!");
                setimageUrl('');
                setNome('');
                setDescricao('');
                setIso('');
                setCategoriaid('');
            })
            .catch(error => {
                console.error("Erro ao enviar dados:", error);
                alert("Falha ao adicionar distro.");
            });
    }

    if (loadingCategorias) {
        return <Loading />; // Exibe o loading até carregar as categorias
    }

    return (
        <div className="login-wrapper">
            <div className="containerLogin">

                <form onSubmit={handleRegister}>
                    <label>Adicionar Distro</label><br />

                    <input
                        placeholder="imageUrl"
                        value={imageUrl}
                        onChange={(e) => setimageUrl(e.target.value)}
                        required
                        maxLength={1000}
                    /><br /><br />

                    <input
                        placeholder="Nome"
                        value={nome}
                        onChange={(e) => setNome(e.target.value)}
                        required
                        maxLength={100}
                    /><br /><br />

                    <input
                        placeholder="Descrição"
                        value={descricao}
                        onChange={(e) => setDescricao(e.target.value)}
                        required
                        maxLength={1000}
                    /><br /><br />

                    <input
                        placeholder="Iso url"
                        value={iso}
                        onChange={(e) => setIso(e.target.value)}
                        required
                        maxLength={1000}
                    /><br /><br />

                    <select
                        value={categoriaid}
                        onChange={(e) => setCategoriaid(e.target.value)}
                        required
                    >
                        <option value="" disabled>Selecione uma categoria</option>
                        {categorias.map(categoria => (
                            <option key={categoria.categoriaId} value={categoria.categoriaId}>
                                {categoria.nome}
                            </option>
                        ))}
                    </select><br /><br />

                    <button type="submit">Adicionar</button>
                </form>
            </div>
        </div>
    );
}

export default AdicionarDistro;
