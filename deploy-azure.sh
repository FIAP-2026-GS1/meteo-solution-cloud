#!/bin/bash
# =============================================================
# Sentinel Alert — Meteo Solution
# Deploy: VM no Azure com Docker + Docker Compose
# Disciplina: DevOps Tools & Cloud Computing — FIAP 2TDS
# RM: 562822
# =============================================================

set -e  # Para execução imediatamente se qualquer comando falhar

# ------------------------------------------
# VARIÁVEIS DE CONFIGURAÇÃO
# ------------------------------------------
RESOURCE_GROUP="rg-meteosolution-gs"
LOCATION="canadacentral"
VM_NAME="vm-rm562822-meteosolution"
VM_SIZE="Standard_B2als_v2"
ADMIN_USER="azureuser"
IMAGE="Ubuntu2204"
PORT_APP="8080"
PORT_SSH="22"
NSG_NAME="nsg-rm562822-meteosolution"
REPO_URL="https://github.com/FIAP-2026-GS1/meteo-solution-cloud.git"

echo ""
echo "=============================================="
echo "  Sentinel Alert — Deploy Azure"
echo "  Meteo Solution | RM: 562822"
echo "=============================================="
echo ""

# ------------------------------------------
# 1. LOGIN (verifica se já está logado)
# ------------------------------------------
echo "[1/6] Verificando autenticação na Azure..."
az account show > /dev/null 2>&1 || az login
az account set --subscription "Azure for Students"
echo "      Subscription: Azure for Students ✓"

# ------------------------------------------
# 2. RESOURCE GROUP
# ------------------------------------------
echo ""
echo "[2/6] Criando Resource Group: $RESOURCE_GROUP..."
az group create \
  --name "$RESOURCE_GROUP" \
  --location "$LOCATION" \
  --output none
echo "      Resource Group criado em $LOCATION ✓"

# ------------------------------------------
# 3. CRIAR A VM
# ------------------------------------------
echo ""
echo "[3/6] Criando VM Linux (Ubuntu 22.04)..."
echo "      Tamanho: $VM_SIZE | Usuário: $ADMIN_USER"
echo "      Aguarde — isso pode levar 1-2 minutos..."

az vm create \
  --resource-group "$RESOURCE_GROUP" \
  --name "$VM_NAME" \
  --image "$IMAGE" \
  --size "$VM_SIZE" \
  --admin-username "$ADMIN_USER" \
  --generate-ssh-keys \
  --output none

echo "      VM criada ✓"

# ------------------------------------------
# 4. ABRIR PORTAS NO FIREWALL (NSG)
# ------------------------------------------
echo ""
echo "[4/6] Configurando regras de firewall (NSG)..."

az vm open-port \
  --resource-group "$RESOURCE_GROUP" \
  --name "$VM_NAME" \
  --port "$PORT_APP" \
  --priority 1001 \
  --output none

echo "      Porta $PORT_APP (API) aberta ✓"
echo "      Porta $PORT_SSH (SSH) já aberta por padrão ✓"

# ------------------------------------------
# 5. INSTALAR DOCKER + DOCKER COMPOSE NA VM
# ------------------------------------------
echo ""
echo "[5/6] Instalando Docker e Docker Compose na VM..."
echo "      Aguarde — isso pode levar 2-3 minutos..."

az vm run-command invoke \
  --resource-group "$RESOURCE_GROUP" \
  --name "$VM_NAME" \
  --command-id RunShellScript \
  --scripts "
    set -e
    apt-get update -y
    apt-get install -y ca-certificates curl gnupg lsb-release git

    # Docker official GPG key
    install -m 0755 -d /etc/apt/keyrings
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | gpg --dearmor -o /etc/apt/keyrings/docker.gpg
    chmod a+r /etc/apt/keyrings/docker.gpg

    # Docker repository
    echo \"deb [arch=\$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.gpg] https://download.docker.com/linux/ubuntu \$(lsb_release -cs) stable\" | tee /etc/apt/sources.list.d/docker.list > /dev/null

    # Instalar Docker Engine + Compose plugin
    apt-get update -y
    apt-get install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

    # Habilitar Docker no boot
    systemctl enable docker
    systemctl start docker

    # Permitir azureuser usar Docker sem sudo
    usermod -aG docker azureuser

    echo 'Docker instalado com sucesso'
    docker --version
    docker compose version
  " \
  --output none

echo "      Docker e Docker Compose instalados ✓"

# ------------------------------------------
# 6. OBTER IP PÚBLICO
# ------------------------------------------
echo ""
echo "[6/6] Obtendo IP público da VM..."

PUBLIC_IP=$(az vm show \
  --resource-group "$RESOURCE_GROUP" \
  --name "$VM_NAME" \
  --show-details \
  --query "publicIps" \
  --output tsv)

# ------------------------------------------
# RESUMO FINAL
# ------------------------------------------
echo ""
echo "=============================================="
echo "  DEPLOY CONCLUÍDO COM SUCESSO!"
echo "=============================================="
echo ""
echo "  VM:            $VM_NAME"
echo "  IP Público:    $PUBLIC_IP"
echo "  Usuário SSH:   $ADMIN_USER"
echo ""
echo "----------------------------------------------"
echo "  PRÓXIMOS PASSOS:"
echo "----------------------------------------------"
echo ""
echo "  1. Conectar na VM via SSH:"
echo "     ssh $ADMIN_USER@$PUBLIC_IP"
echo ""
echo "  2. Dentro da VM, clonar e subir o projeto:"
echo "     git clone $REPO_URL"
echo "     cd meteo-solution-cloud"
echo "     docker compose up -d"
echo ""
echo "  3. Aguardar containers subirem (~30s) e acessar:"
echo "     http://$PUBLIC_IP:$PORT_APP/swagger"
echo ""
echo "  4. Para apagar tudo depois: ./destroy-azure.sh"
echo ""
echo "=============================================="
