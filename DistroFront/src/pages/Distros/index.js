import { useEffect, useState } from "react";
import './styles.css';
import Loading from '../../components/Loading'; // <--- Importando o componente

function Distros() {
    const [distro, setDistro] = useState([]);
    const [loading, setLoading] = useState(true); // <--- Novo estado

    useEffect(() => {
        fetch("http://localhost:5177/api/Distros")
            .then((r) => r.json())
            .then((json) => {
                setDistro(json);
                setLoading(false); // <--- Quando os dados carregam, desativa o loading
            })
            .catch(error => {
                console.error("Erro ao carregar distros:", error);
                setLoading(false); // Mesmo com erro, precisamos sair do loading
            });
    }, []);

    if (loading) {
        return <Loading />; // <--- Exibe enquanto carrega
    }

    return (
        <div className="container">
            {distro.map((item) => {
                return (
                    <article key={item.distroId} className="posts">
                        <strong className="titulo">{item.nome}</strong>
                        <img className="capa" src={item.imageUrl} alt={item.nome} />
                        <p className="subtitulo">{item.descricao}</p>
                        <p className="subtitulo"><strong>Categoria: </strong>{item.categoria.nome}</p>
                        <a className="botao"
                            href={item.iso}
                            target="_blank"
                            rel="noopener noreferrer">
                            Baixar a ISO
                        </a>
                    </article>
                );
            })}
        </div>
    );
}

export default Distros;
