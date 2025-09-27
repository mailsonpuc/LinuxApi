import { useState, useEffect, type FormEvent } from "react";
import './adddistro.css';
import Loading from "../../components/loading";
import { type Categoria } from "../../interfaces/Icategoria";



export function AdicionarDistro() {
    // Estados para os dados da nova distro
    const [imageUrl, setimageUrl] = useState('');
    const [nome, setNome] = useState('');
    const [descricao, setDescricao] = useState('');
    const [iso, setIso] = useState('');
    const [categoriaid, setCategoriaid] = useState(''); // O ID da categoria selecionada

    // Estados para o carregamento das categorias
    const [categorias, setCategorias] = useState<Categoria[]>([]);
    const [loadingCategorias, setLoadingCategorias] = useState(true);

    // Efeito para carregar a lista de categorias ao montar o componente
    useEffect(() => {
        setLoadingCategorias(true);

        fetch("http://localhost:5177/api/Categorias")
            .then(response => response.json())
            .then((data: Categoria[]) => {
                setCategorias(data);
                setLoadingCategorias(false);
            })
            .catch(error => {
                console.error("Erro ao buscar categorias:", error);
                setLoadingCategorias(false);
            });
    }, []);

    // Função de tratamento do envio do formulário
    function handleRegister(e: FormEvent<HTMLFormElement>) {
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
                    // Tenta ler o erro da resposta, se disponível
                    return response.json().then(err => {
                        throw new Error(err.message || "Erro ao adicionar distro.");
                    });
                }
                return response.json();
            })
            .then(data => {
                alert("Distro adicionada com sucesso!");
                // Limpa os campos após o sucesso
                setimageUrl('');
                setNome('');
                setDescricao('');
                setIso('');
                setCategoriaid('');
            })
            .catch(error => {
                console.error("Erro ao enviar dados:", error);
                alert(`Falha ao adicionar distro: ${error.message}`);
            });
    }

    // Exibe o componente Loading enquanto as categorias são carregadas
    if (loadingCategorias) {
        return <Loading />;
    }

    return (
        <div className="login-wrapper">
            <div className="containerLogin">

                <form onSubmit={handleRegister}>
                    <label>Adicionar Distro</label><br />

                    {/* INPUT: Image URL */}
                    <input
                        placeholder="URL da Imagem"
                        value={imageUrl}
                        onChange={(e) => setimageUrl(e.target.value)}
                        required
                        maxLength={1000}
                    /><br /><br />

                    {/* INPUT: Nome */}
                    <input
                        placeholder="Nome da Distro"
                        value={nome}
                        onChange={(e) => setNome(e.target.value)}
                        required
                        maxLength={100}
                    /><br /><br />

                    {/* INPUT: Descrição */}
                    <input
                        placeholder="Descrição"
                        value={descricao}
                        onChange={(e) => setDescricao(e.target.value)}
                        required
                        maxLength={1000}
                    /><br /><br />

                    {/* INPUT: ISO URL */}
                    <input
                        placeholder="URL da ISO"
                        value={iso}
                        onChange={(e) => setIso(e.target.value)}
                        required
                        maxLength={1000}
                    /><br /><br />

                    {/* SELECT: Categoria */}
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