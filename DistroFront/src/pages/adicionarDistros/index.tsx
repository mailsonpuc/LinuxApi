import { useState, useEffect, type FormEvent } from "react";
import './adddistro.css';
import Loading from "../../components/loading";
import { type Categoria } from "../../interfaces/Icategoria";

// Endereços da API 
const API_URL = "https://apireact-dcarhnh2g3dtdehf.brazilsouth-01.azurewebsites.net/api";

export function AdicionarDistro() {
    // Estados para os dados da nova distro
    const [imageUrl, setimageUrl] = useState('');
    const [nome, setNome] = useState('');
    const [descricao, setDescricao] = useState('');
    const [iso, setIso] = useState('');
    const [categoriaid, setCategoriaid] = useState(''); 

    // Estados para o carregamento das categorias
    const [categorias, setCategorias] = useState<Categoria[]>([]);
    const [loadingCategorias, setLoadingCategorias] = useState(true);

    // Efeito para carregar a lista de categorias ao montar o componente
    useEffect(() => {
        setLoadingCategorias(true);

        fetch(`${API_URL}/Categorias`)
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
    async function handleRegister(e: FormEvent<HTMLFormElement>) {
        e.preventDefault();

        const novaDistro = {
            imageUrl: imageUrl,
            nome: nome,
            descricao: descricao,
            iso: iso,
            categoriaId: categoriaid // Usando string (GUID)
        };

        try {
            const response = await fetch(`${API_URL}/Distros`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(novaDistro)
            });

            if (!response.ok) {
                // Tenta ler o erro da resposta para lançar uma exceção
                const err = await response.json();
                throw new Error(err.message || "Erro ao adicionar distro.");
            }
            
         
            alert("Distro adicionada com sucesso! 🎉");
            
            // Limpa os campos após o sucesso
            setimageUrl('');
            setNome('');
            setDescricao('');
            setIso('');
            setCategoriaid('');

        } catch (error: any) {
            console.error("Erro ao enviar dados:", error);
            alert(`Falha ao adicionar distro: ${error.message}`);
        }
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
                        // Tipagem  para o evento de mudança
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setimageUrl(e.target.value)}
                        required
                        maxLength={1000}
                    /><br /><br />

                    {/* INPUT: Nome */}
                    <input
                        placeholder="Nome da Distro"
                        value={nome}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setNome(e.target.value)}
                        required
                        maxLength={100}
                    /><br /><br />

                    {/* INPUT: Descrição */}
                    <input
                        placeholder="Descrição"
                        value={descricao}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setDescricao(e.target.value)}
                        required
                        maxLength={1000}
                    /><br /><br />

                    {/* INPUT: ISO URL */}
                    <input
                        placeholder="URL da ISO"
                        value={iso}
                        onChange={(e: React.ChangeEvent<HTMLInputElement>) => setIso(e.target.value)}
                        required
                        maxLength={1000}
                    /><br /><br />

                    {/* SELECT: Categoria */}
                    <select
                        value={categoriaid}
                        onChange={(e: React.ChangeEvent<HTMLSelectElement>) => setCategoriaid(e.target.value)}
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
