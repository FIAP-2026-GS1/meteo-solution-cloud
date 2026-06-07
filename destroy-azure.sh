#!/bin/bash
# =============================================================
# Sentinel Alert — Meteo Solution
# Destroy: Remove todos os recursos criados no Azure
# Disciplina: DevOps Tools & Cloud Computing — FIAP 2TDS
# RM: 562822
# =============================================================

RESOURCE_GROUP="rg-meteosolution-gs"

echo ""
echo "=============================================="
echo "  Sentinel Alert — Destroy Azure"
echo "  Meteo Solution | RM: 562822"
echo "=============================================="
echo ""
echo "  ATENÇÃO: Isso irá apagar TODOS os recursos"
echo "  do Resource Group: $RESOURCE_GROUP"
echo ""
read -p "  Tem certeza? Digite 'sim' para confirmar: " CONFIRM
echo ""

if [ "$CONFIRM" != "sim" ]; then
  echo "  Operação cancelada."
  exit 0
fi

echo "[1/2] Verificando autenticação na Azure..."
az account show > /dev/null 2>&1 || az login
az account set --subscription "Azure for Students"
echo "      Subscription: Azure for Students ✓"

echo ""
echo "[2/2] Apagando Resource Group e todos os recursos..."
echo "      Aguarde — isso pode levar 2-3 minutos..."

az group delete \
  --name "$RESOURCE_GROUP" \
  --yes \
  --no-wait \
  --output none

echo ""
echo "=============================================="
echo "  DESTROY INICIADO COM SUCESSO!"
echo "=============================================="
echo ""
echo "  O Resource Group '$RESOURCE_GROUP' está sendo"
echo "  removido em background pela Azure."
echo ""
echo "  Recursos removidos:"
echo "  - VM: vm-rm562822-meteosolution"
echo "  - NSG, IP Público, Disco, Rede Virtual"
echo ""
echo "  Seus créditos Azure for Students estão protegidos."
echo "=============================================="
