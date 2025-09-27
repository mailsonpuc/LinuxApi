// src/interfaces/Idistros.tsx

// Não precisa importar Categoria aqui, pois ela não está aninhada

export interface Distro {
    distroId: string;
    imageUrl: string;
    nome: string;
    descricao: string;
    iso: string;
    // CORREÇÃO: Usar apenas categoriaId (string/GUID)
    categoriaId: string; 
    
    // Remova a linha 'categoria: Categoria;'
}